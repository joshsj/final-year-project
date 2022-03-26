﻿using Microsoft.EntityFrameworkCore;
using RendezVous.Domain.Entities;

namespace RendezVous.Application.Common.Interfaces;

public interface IRendezVousDbContext
{
    DbSet<Employee> Employees { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}