using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;


namespace Joho.Web.API.Filters.JwtAuthentication
{
    public class AuthenticationFailureResult : IHttpActionResult
    {
        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
        {
            ReasonPhrase = reasonPhrase;
            Request = request;
        }

        public string ReasonPhrase { get; }

        public HttpRequestMessage Request { get; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                RequestMessage = Request,
                ReasonPhrase = ReasonPhrase,
              //  Content = new StringContent(Utility.ResultStatus(false, ReasonPhrase, string.Empty))
              Content=new StringContent (ReasonPhrase)
            };

            return response;
        }
    }
}
