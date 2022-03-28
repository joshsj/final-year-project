
namespace RendezVous.Domain.Options;

public class Auth0Options
{
    public string Domain { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string Authority => $"https://{Domain}/";

    /// <summary>Custom claim assigned to the access token</summary>
    public string NameClaim { get; set; } = default!;
    
    /// <summary>Custom claim assigned to the access token</summary>
    public string EmailClaim { get; set; } = default!;
}
