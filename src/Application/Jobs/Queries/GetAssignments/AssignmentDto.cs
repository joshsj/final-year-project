using RendezVous.Application.Common.Dtos;

namespace RendezVous.Application.Jobs.Queries.GetAssignments;

public class AssignmentDto : EntityDto
{
    public string EmployeeProviderId { get; set; } = null!;
    public string EmployeeName { get; set; } = null!;
    
    public bool HasClockedIn { get; set; }
    public bool HasClockedOut { get; set; }
}
