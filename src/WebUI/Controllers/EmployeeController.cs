using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace RendezVous.WebUI.Controllers;

public class EmployeeController : RendezVousControllerBase
{
    [HttpPut]
    [Authorize]
    public async Task<ActionResult> Put()
    {
        throw new NotImplementedException();
    }
}