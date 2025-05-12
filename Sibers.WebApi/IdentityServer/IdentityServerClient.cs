namespace Sibers.WebApi.IdentityServer;

public class IdentityServerClient
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public List<string> AllowedGrantTypes { get; set; }
    public List<string> AllowedScopes { get; set; }
}
