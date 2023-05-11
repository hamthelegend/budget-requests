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
    
    private DbSet<User> Users { get; set; }
    private DbSet<AdminRole> AdminRoles { get; set; }
    private DbSet<OfficerRole> OfficerRoles { get; set; }
    private DbSet<Organization> Organizations { get; set; }
    private DbSet<Preference> Preferences { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(CONNECTION_STRING);
    }

    public IEnumerable<User> GetUsers()
    {
        var users = Users.Select(x => x as User).ToList();
        return users;
    }

    public User? GetUser(int id)
    {
        return GetUsers().FirstOrDefault(x => x.Id == id);
    }
    
    public bool AddUser(User user)
    {
        Users.Add(user);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public List<User> GetAdmins()
    {
        return Users.Where(x => x.Type == UserType.Admin).ToList();
    }

    public List<AdminRole> GetAdminRoles(User admin)
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

    public List<User> GetOfficers()
    {
        return Users.Where(x => x.Type == UserType.Officer).ToList();
    }

    public List<OfficerRole> GetOfficerRoles(User officer)
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