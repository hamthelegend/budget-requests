using BudgetRequests.Models.Admins;
using BudgetRequests.Models.BudgetRequests;
using BudgetRequests.Models.Organizations;
using Microsoft.EntityFrameworkCore;

namespace BudgetRequests.Models;

public class DatabaseContext : DbContext
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

    private DbSet<BudgetRequest> BudgetRequests { get; set; }
    private DbSet<Expense> Expenses { get; set; }
    private DbSet<AdminSignatory> AdminSignatories { get; set; }
    private DbSet<OfficerSignatory> OfficerSignatories { get; set; }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<OfficerRole>()
    //         .HasOne(officerRole => officerRole.Organization)
    //         .WithMany()
    //         .HasForeignKey(officerRole => officerRole.OrganizationId)
    //         .IsRequired();
    // }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(CONNECTION_STRING);
    }

    public List<User> GetUsers()
    {
        var users = Users.ToList();
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

    public bool RemoveUser(User user)
    {
        Users.Remove(user);
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

    public bool RemoveAdminRole(AdminRole adminRole)
    {
        AdminRoles.Remove(adminRole);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public List<User> GetOfficers()
    {
        return Users.Where(x => x.Type == UserType.Officer).ToList();
    }

    public List<OfficerRole> GetOfficerRoles(User officer)
    {
        return OfficerRoles
            .Where(x => x.Officer == officer)
            .Include(officerRole => officerRole.Organization)
            .ToList();
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

    public bool RemoveOfficerRole(OfficerRole officerRole)
    {
        OfficerRoles.Remove(officerRole);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public List<Organization> GetOrganizations()
    {
        return Organizations.ToList();
    }

    public List<Organization> GetOrganizations(User user)
    {
        return /*user.Type == UserType.Admin
            ? Organizations.Where(organization => organization.Adviser == user).ToList()
            : */OfficerRoles.Where(officerRole => officerRole.Officer == user).Select(x => x.Organization).ToList();
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

    public bool RemoveOrganization(Organization organization)
    {
        Organizations.Remove(organization);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public User? GetAdmin(AdminPosition position)
    {
        return AdminRoles.FirstOrDefault(x => x.Position == position)?.Admin;
    }

    public List<BudgetRequest> GetBudgetRequestsRequested(User user)
    {
        var organizations = GetOrganizations(user);
        return BudgetRequests.Where(budgetRequest => organizations.Contains(budgetRequest.Requester)).ToList();
    }

    public List<BudgetRequest> GetBudgetRequests(User user)
    {
        if (user.Type == UserType.Officer)
        {
            return OfficerSignatories
                .Where(signatory => signatory.Role.Officer == user)
                .Select(signatory => signatory.BudgetRequest).ToList();
        }
        
        var budgetRequests = AdminSignatories
            .Where(signatory => signatory.Role.Admin == user)
            .Select(signatory => signatory.BudgetRequest);
        var budgetRequestsToShow = new List<BudgetRequest>();
        foreach (var budgetRequest in budgetRequests)
        {
            var officerSignatories =
                OfficerSignatories.Where(signatory => signatory.BudgetRequest == budgetRequest);
            var adminSignatories = 
                AdminSignatories.Where(signatory => signatory.BudgetRequest == budgetRequest);
            
            var assistantDeanSignatory = adminSignatories
                .FirstOrDefault(signatory => signatory.Role.Position == AdminPosition.AssistantDean);
            var deanSignatory = adminSignatories
                .FirstOrDefault(signatory => signatory.Role.Position == AdminPosition.AssistantDean);
            var studentAffairsDirectorSignatory = adminSignatories
                .FirstOrDefault(signatory => signatory.Role.Position == AdminPosition.AssistantDean);
                
            var allOfficersHaveSigned = officerSignatories.All(x => x.HasSigned);
                
            if ((user == assistantDeanSignatory?.Role.Admin && allOfficersHaveSigned) ||
                (user == deanSignatory?.Role.Admin && assistantDeanSignatory?.HasSigned == true) ||
                (user == studentAffairsDirectorSignatory?.Role.Admin && deanSignatory?.HasSigned == true))
            {
                budgetRequestsToShow.Add(budgetRequest);
            }
        }

        return budgetRequestsToShow;
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