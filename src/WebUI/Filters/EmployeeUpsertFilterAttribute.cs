﻿using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RendezVous.Application.Common.Interfaces;
using RendezVous.Domain.Entities;
using RendezVous.Domain.Options;

namespace RendezVous.WebUI.Filters;

public class EmployeeUpsertFilterAttribute : ActionFilterAttribute
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRendezVousDbContext _rendezVousDbContext;
    private readonly Auth0Options _auth0Options;

    public EmployeeUpsertFilterAttribute(
        ICurrentUserService currentUserService,
        IHttpContextAccessor httpContextAccessor,
        IRendezVousDbContext rendezVousDbContext,
        IOptions<Auth0Options> auth0Options)
    {
        _currentUserService = currentUserService;
        _httpContextAccessor = httpContextAccessor;
        _rendezVousDbContext = rendezVousDbContext;
        _auth0Options = auth0Options.Value;
    }
    
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var currentProviderId = _currentUserService.ProviderId;
        var user = _httpContextAccessor?.HttpContext?.User;

        if (currentProviderId is null || user is null)
        {
            return;
        }

        var employee = await _rendezVousDbContext.Employees.FirstOrDefaultAsync(x => x.ProviderId == currentProviderId);
        
        if (employee is null)
        {
            employee = new Employee
            {
                Id = Guid.NewGuid(), 
                ProviderId = currentProviderId,
            };

            _rendezVousDbContext.Employees.Add(employee);
        }

        employee.Name = user.FindFirstValue(_auth0Options.NameClaim);
        employee.Email = user.FindFirstValue(_auth0Options.EmailClaim);

        await _rendezVousDbContext.SaveChangesAsync(CancellationToken.None);
    }
}
