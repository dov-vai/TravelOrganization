using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TravelOrganization.Data.Services
{
    public class BasicAuthMessageHandler : DelegatingHandler
    {
        private readonly IBasicAuthCredentials _credentials;

        public BasicAuthMessageHandler(IBasicAuthCredentials credentials)
        {
            _credentials = credentials;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(_credentials.AuthorizationHeader))
            {
                request.Headers.Authorization = AuthenticationHeaderValue.Parse(_credentials.AuthorizationHeader);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
