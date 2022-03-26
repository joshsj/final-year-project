
namespace RendezVous.Domain.Options;

public class Auth0Options
{
    public string Domain { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string Authority => $"https://{Domain}/";
}