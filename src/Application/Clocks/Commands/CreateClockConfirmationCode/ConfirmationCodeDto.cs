namespace RendezVous.Application.Clocks.Commands.CreateClockConfirmationCode;

public class ConfirmationCodeDto
{
    public string SvgSource { get; set; } = null!;
    public int TimeRemaining { get; set; }
}
