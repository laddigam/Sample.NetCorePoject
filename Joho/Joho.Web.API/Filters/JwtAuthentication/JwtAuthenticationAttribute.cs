using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;


namespace Joho.Web.API.Filters.JwtAuthentication
{
    public class JwtAuthenticationAttribute : Attribute, IAuthenticationFilter

    {

        public string Realm { get; set; }
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {

            var request = context.Request;
            var authorization = request.Headers.Authorization;


            if (authorization == null || authorization.Scheme != "Bearer")
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing Jwt Token", request);
                return;
            }
            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing Jwt Token", request);
                return;
            }
            var token = authorization.Parameter;

            var simplePrinciple = JwtManager.GetPrincipal(token);

            if (simplePrinciple != null)
            {
                var identity = simplePrinciple.Identity as ClaimsIdentity;


                if (identity != null)
                {
                    var usernameClaim = identity.FindFirst(ClaimTypes.Name);
                    string UserId = usernameClaim?.Value;
                }
            }

            var principal = await AuthenticateJwtToken(token, context);

            if (principal == null)
                context.ErrorResult = new AuthenticationFailureResult("Invalid token", request);

            else
                context.Principal = principal;
        }



        private static bool ValidateToken(string token, HttpAuthenticationContext context, out int userid)
        {
            userid = 0;

            var simplePrinciple = JwtManager.GetPrincipal(token);

            if (simplePrinciple == null)
                return false;
            var identity = simplePrinciple.Identity as ClaimsIdentity;

            if (identity == null)
                return false;

            if (!identity.IsAuthenticated)
                return false;

            var usernameClaim = identity.FindFirst(ClaimTypes.Name);
            userid = Convert.ToInt32(usernameClaim?.Value.Split(':')[0]);

            if (userid == 0)
                return false;

            //LoginAuthenticationRepository objLogin = new LoginAuthenticationRepository();
            string password = "j";//objLogin.GetUserDataValid(userid);

            if (password == "")
                return false;
            return true;
            // More validate to check whether username exists in system
        }

        protected Task<IPrincipal> AuthenticateJwtToken(string token, HttpAuthenticationContext context)
        {
            int userid;

            if (ValidateToken(token, context, out userid))
            {
                // based on username to get more information from database in order to build local identity
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userid.ToString())
                    // Add more claims if needed: Roles, ...
                };

                var identity = new ClaimsIdentity(claims, "Jwt");
                IPrincipal user = new ClaimsPrincipal(identity);

                return Task.FromResult(user);
            }

            return Task.FromResult<IPrincipal>(null);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter = null;

            if (!string.IsNullOrEmpty(Realm))
                parameter = "realm=\"" + Realm + "\"";

            context.ChallengeWith("Bearer", parameter);
        }
    }
}
