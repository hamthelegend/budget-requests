using BudgetRequests.Models.Admins;
using BudgetRequests.Models.Organizations;
using Microsoft.EntityFrameworkCore;

namespace BudgetRequests.Models;

public class DatabaseContext: DbContext
{
    public DbSet<Admin> Admins { get; set; }
    public DbSet<AdminRole> AdminRoles { get; set; }
    
    public DbSet<Officer> Officers { get; set; }
    public DbSet<OfficerRole> OfficerRoles { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    
    public DbSet<Preference> Preferences { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=(localdb)\mssqllocaldb;Database=BudgetRequests;Trusted_Connection=True");
    }
}