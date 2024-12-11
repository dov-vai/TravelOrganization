namespace TravelOrganization.Data.Services
{
    public interface IBasicAuthCredentials
    {
        string AuthorizationHeader { get; set; }
    }
}
