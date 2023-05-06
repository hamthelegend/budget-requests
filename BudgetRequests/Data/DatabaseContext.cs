using BudgetRequests.Models.Admins;
using BudgetRequests.Models.Organizations;
using Microsoft.EntityFrameworkCore;

namespace BudgetRequests.Models;

public class DatabaseContext: DbContext
{

    public const string CONNECTION_STRING =
        @"Server=(localdb)\mssqllocaldb;Database=BudgetRequests;Trusted_Connection=True";
    
    public DatabaseContext (DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<AdminRole> AdminRoles { get; set; }
    
    public DbSet<Officer> Officers { get; set; }
    public DbSet<OfficerRole> OfficerRoles { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    
    public DbSet<Preference> Preferences { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(CONNECTION_STRING);
    }
}