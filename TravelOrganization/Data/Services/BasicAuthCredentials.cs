namespace TravelOrganization.Data.Services
{
    public class BasicAuthCredentials : IBasicAuthCredentials
    {
        public string AuthorizationHeader { get; set; }
    }
}
