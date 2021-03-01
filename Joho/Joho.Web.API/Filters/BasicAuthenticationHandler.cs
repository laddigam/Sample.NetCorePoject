using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Joho.Web.API.Filters.JwtAuthentication;

namespace Joho.Web.API.Filters
{
    public  class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public int userxid;
 
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock )
            : base(options, logger, encoder, clock)
        {
         
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");
            if (!Request.Headers.ContainsKey("Owner"))
                return AuthenticateResult.Fail("Missing Owner Header");


            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

                if (authHeader == null || authHeader.Scheme != "Bearer")
                {
                    return AuthenticateResult.Fail("Missing Jwt Token");

                }
                if (string.IsNullOrEmpty(authHeader.Parameter))
                {
                    return AuthenticateResult.Fail("Missing Jwt Token");

                }
                var token = authHeader.Parameter;
                ///Owner means current user id.

                string Owner = Convert.ToString(Request.Headers["Owner"]);
                if (string.IsNullOrEmpty(Owner))
                {
                    return AuthenticateResult.Fail("Missing owner");

                }
                //var simplePrinciple = JwtManager.GetPrincipal(token);

                //if (simplePrinciple != null)
                //{
                //    var identity = simplePrinciple.Identity as ClaimsIdentity;


                //    if (identity != null)
                //    {
                //        var usernameClaim = identity.FindFirst(ClaimTypes.Name);
                //        string UserId = usernameClaim?.Value;
                //    }
                //}

                var principal = await AuthenticateJwtToken(token, Owner);



                //var claims = new[] {
                //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                //    new Claim(ClaimTypes.Name, user.Username),
                //};
                //var identity = new ClaimsIdentity(claims, Scheme.Name);
                //var principal = new ClaimsPrincipal(identity);

                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

        }

        private static bool ValidateToken(string token, string currentUserId, out int userid)
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
            if (currentUserId != Convert.ToBase64String(Encoding.UTF8.GetBytes(userid.ToString())))
            {
                return false;
            }
            return true;
            // More validate to check whether username exists in system
        }

        protected Task<ClaimsPrincipal> AuthenticateJwtToken(string token, string currentUserId)
        {
            int userid;

            if (ValidateToken(token, currentUserId, out userid))
            {
                // based on username to get more information from database in order to build local identity
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userid.ToString()),
                      new Claim(ClaimTypes.Authentication,token)
                    // Add more claims if needed: Roles, ...
                };

                var identity = new ClaimsIdentity(claims, "Jwt");
                ClaimsPrincipal user = new ClaimsPrincipal(identity);

                return Task.FromResult(user);
            }

            return Task.FromResult<ClaimsPrincipal>(null);
        }
    }
} 