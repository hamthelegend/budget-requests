using BudgetRequests.Models.Admins;
using BudgetRequests.Models.Organizations;
using Microsoft.EntityFrameworkCore;

namespace BudgetRequests.Models;

public class DatabaseContext: DbContext
{

    public const string CONNECTION_STRING =
        @"Server=(localdb)\mssqllocaldb;Database=BudgetRequests;Trusted_Connection=True";
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }
    
    private DbSet<Admin> Admins { get; set; }
    private DbSet<AdminRole> AdminRoles { get; set; }
    
    private DbSet<Officer> Officers { get; set; }
    private DbSet<OfficerRole> OfficerRoles { get; set; }
    private DbSet<Organization> Organizations { get; set; }
    
    private DbSet<Preference> Preferences { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(CONNECTION_STRING);
    }

    public List<User> GetUsers()
    {
        var users = Admins.Select(x => x as User).ToList();
        var officers = Officers.Select(x => x as User).ToList();
        users.AddRange(officers);
        return users;
    }

    public User? GetUser(int id)
    {
        return GetUsers().FirstOrDefault(x => x.Id == id);
    }

    public List<Admin> GetAdmins()
    {
        return Admins.ToList();
    }

    public Admin? GetAdmin(int id)
    {
        return Admins.FirstOrDefault(x => x.Id == id);
    }

    public bool AddAdmin(Admin admin)
    {
        Admins.Add(admin);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public List<AdminRole> GetAdminRoles(Admin admin)
    {
        return AdminRoles.Where(x => x.Admin == admin).ToList();
    }

    public bool HasSuperAdmin()
    {
        return AdminRoles.Any(x => x.Position == AdminPosition.SuperAdmin);
    }
    
    public AdminRole? GetAdminRole(int id)
    {
        return AdminRoles.FirstOrDefault(x => x.Id == id);
    }

    public bool AddAdminRole(AdminRole adminRole)
    {
        AdminRoles.Add(adminRole);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public List<Officer> GetOfficers()
    {
        return Officers.ToList();
    }

    public Officer? GetOfficer(int id)
    {
        return Officers.FirstOrDefault(x => x.Id == id);
    }
    
    public bool AddOfficer(Officer officer)
    {
        Officers.Add(officer);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public List<OfficerRole> GetOfficerRoles(Officer officer)
    {
        return OfficerRoles.Where(x => x.Officer == officer).ToList();
    }

    public OfficerRole? GetOfficerRole(int id)
    {
        return OfficerRoles.FirstOrDefault(x => x.Id == id);
    }
    
    public bool AddOfficerRole(OfficerRole officerRole)
    {
        OfficerRoles.Add(officerRole);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public List<Organization> GetOrganizations()
    {
        return Organizations.ToList();
    }

    public Organization? GetOrganization(int id)
    {
        return Organizations.FirstOrDefault(x => x.Id == id);
    }
    
    public bool AddOrganization(Organization organization)
    {
        Organizations.Add(organization);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public Preference? GetPreference()
    {
        return Preferences.LastOrDefault();
    }

    public bool SetPreference(Preference preference)
    {
        Preferences.Add(preference);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }
    
}