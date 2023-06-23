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
    private DbSet<TemporaryExpense> TemporaryExpenses { get; set; }
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

    public bool IsSuperAdmin(User user)
    {
        return AdminRoles.Any(adminRole => adminRole.Admin == user);
    }

    public bool CanCreateRequests(User user)
    {
        var organizations = GetOfficerOrganizations(user);
        return user.Type == UserType.Officer && organizations.Count > 0;
    }
    
    public bool CanUserDeleteBudgetRequest(User user, BudgetRequest budgetRequest)
    {
        var users = OfficerRoles
            .Where(officerRole => officerRole.Organization == budgetRequest.Requester)
            .Include(officerRole => officerRole.Officer)
            .Select(officerRole => officerRole.Officer)
            .ToList();
        users.Add(budgetRequest.Requester.Adviser!);
        return users.Contains(user);
    }

    public bool AreCollegeAdminsSet()
    {
        var collegeAdmins = GetCollegeAdmins();
        return collegeAdmins.ToList().All(admin => admin != null);
    }

    public User? GetUser(string username)
    {
        return Users.FirstOrDefault(user => user.Username == username);
    }

    public List<User> GetUsers()
    {
        return Users.ToList();
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

    public CollegeAdmins GetCollegeAdmins()
    {
        var assistantDean = AdminRoles
            .Where(adminRole => adminRole.Position == AdminPosition.AssistantDean)
            .Include(adminRole => adminRole.Admin)
            .OrderBy(adminRole => adminRole.Id)
            .LastOrDefault()
            ?.Admin;
        var dean = AdminRoles
            .Where(adminRole => adminRole.Position == AdminPosition.Dean)
            .Include(adminRole => adminRole.Admin)
            .OrderBy(adminRole => adminRole.Id)
            .LastOrDefault()
            ?.Admin;
        var studentAffairsDirector = AdminRoles
            .Where(adminRole => adminRole.Position == AdminPosition.StudentAffairsDirector)
            .Include(adminRole => adminRole.Admin)
            .OrderBy(adminRole => adminRole.Id)
            .LastOrDefault()
            ?.Admin;
        return new CollegeAdmins(assistantDean, dean, studentAffairsDirector);
    }

    public void SetCollegeAdmins(CollegeAdmins collegeAdmins)
    {
        var oldCollegeAdmins = GetCollegeAdmins();
        
        if (collegeAdmins.AssistantDean != null && collegeAdmins.AssistantDean != oldCollegeAdmins.AssistantDean)
        {
            AdminRoles.Add(new AdminRole
            {
                Admin = collegeAdmins.AssistantDean,
                Position = AdminPosition.AssistantDean,
            });
        }
        
        if (collegeAdmins.Dean != null && collegeAdmins.Dean != oldCollegeAdmins.Dean)
        {
            AdminRoles.Add(new AdminRole
            {
                Admin = collegeAdmins.Dean,
                Position = AdminPosition.Dean,
            });
        }
        
        if (collegeAdmins.StudentAffairsDirector != null && collegeAdmins.StudentAffairsDirector != oldCollegeAdmins.StudentAffairsDirector)
        {
            AdminRoles.Add(new AdminRole
            {
                Admin = collegeAdmins.StudentAffairsDirector,
                Position = AdminPosition.StudentAffairsDirector,
            });
        }

        SaveChanges();
    }

    public AdminRole? GetAdminRole(int id)
    {
        return AdminRoles.FirstOrDefault(x => x.Id == id);
    }

    public AdminRole? GetAdminRole(AdminPosition position)
    {
        return AdminRoles
            .Where(adminRole => adminRole.Position == position)
            .Include(adminRole => adminRole.Admin)
            .OrderBy(adminRole => adminRole.Id)
            .LastOrDefault();
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

    public User? GetOfficer(int id)
    {
        return Users.FirstOrDefault(user => user.Id == id);
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

    /*public bool AddOfficerRole(OfficerRole officerRole)
    {
        OfficerRoles.Add(officerRole);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }*/

    /*public bool RemoveOfficerRole(OfficerRole officerRole)
    {
        OfficerRoles.Remove(officerRole);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }*/

    public OrganizationOfficers GetOrganizationOfficers(Organization organization)
    {
        var officerRoles = OfficerRoles
            .Where(officerRole => officerRole.Organization == organization)
            .Include(officerRole => officerRole.Officer)
            .OrderBy(officerRole => officerRole.Id);
        
        var president =
            officerRoles.LastOrDefault(officerRole => officerRole.Position == OrganizationPosition.President);
        var vicePresident =
            officerRoles.LastOrDefault(officerRole => officerRole.Position == OrganizationPosition.VicePresident);
        var secretary =
            officerRoles.LastOrDefault(officerRole => officerRole.Position == OrganizationPosition.Secretary);
        var treasurer =
            officerRoles.LastOrDefault(officerRole => officerRole.Position == OrganizationPosition.Treasurer);
        var auditor =
            officerRoles.LastOrDefault(officerRole => officerRole.Position == OrganizationPosition.Auditor);
        var publicRelationsOfficer =
            officerRoles.LastOrDefault(officerRole => officerRole.Position == OrganizationPosition.PublicRelationsOfficer);

        return new OrganizationOfficers(
            president, 
            vicePresident, 
            secretary, 
            treasurer, 
            auditor,
            publicRelationsOfficer);
    }

    public void SetOrganizationOfficers(OrganizationOfficers organizationOfficers)
    {
        foreach (var organizationOfficer in organizationOfficers.ToList())
        {
            OfficerRoles.Add(organizationOfficer);
        }

        SaveChanges();
    }

    public List<Organization> GetOrganizations()
    {
        return Organizations
            .Include(organization => organization.Adviser)
            .ToList();
    }

    public List<Organization> GetOrganizations(User user)
    {
        return user.Type == UserType.Admin
            ? Organizations
                .Where(organization => organization.Adviser == user)
                .Include(organization => organization.Adviser)
                .Distinct()
                .ToList()
            : GetOfficerOrganizations(user);
    }

    public List<Organization> GetOfficerOrganizations(User officer)
    {
        return OfficerRoles
            .Where(officerRole => officerRole.Officer == officer)
            .Include(officerRole => officerRole.Organization.Adviser)
            .Select(x => x.Organization)
            .Distinct()
            .ToList();
    }

    public Organization? GetOrganization(int id)
    {
        return Organizations
            .Where(x => x.Id == id)
            .Include(x => x.Adviser)
            .FirstOrDefault();
    }

    public bool AddOrganization(Organization organization)
    {
        Organizations.Add(organization);
        var adviserRole = new AdminRole { Admin = organization.Adviser, Position = AdminPosition.Adviser };
        AdminRoles.Add(adviserRole);
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
        return AdminRoles
            .Where(x => x.Position == position)
            .Include(x => x.Admin)
            .OrderBy(x => x.Id)
            .LastOrDefault()
            ?.Admin;
    }

    public BudgetRequest? GetBudgetRequest(int id)
    {
        return BudgetRequests
            .Where(budgetRequest => budgetRequest.Id == id)
            .Include(budgetRequest => budgetRequest.Requester)
            .Include(budgetRequest => budgetRequest.Requester.Adviser)
            .FirstOrDefault();
    }

    public List<BudgetRequest> GetBudgetRequestsRequested(User user)
    {
        var organizations = GetOrganizations(user);
        return BudgetRequests.Where(budgetRequest => organizations.Contains(budgetRequest.Requester)).ToList();
    }

    public List<BudgetRequest> GetBudgetRequests()
    {
        return BudgetRequests.ToList();
    }

    public List<BudgetRequest> GetBudgetRequests(User user)
    {
        if (user.Type == UserType.Officer)
        {
            var organizations = GetOrganizations(user);
            return BudgetRequests.Where(budgetRequest => organizations.Contains(budgetRequest.Requester)).ToList();
        }

        var budgetRequests = AdminSignatories
            .Where(signatory => signatory.Role.Admin == user)
            .Include(signatory => signatory.BudgetRequest.Requester)
            .Select(signatory => signatory.BudgetRequest)
            .Distinct()
            .ToList();
        var budgetRequestsToShow = new List<BudgetRequest>();
        foreach (var budgetRequest in budgetRequests)
        {
            var signatories = GetSignatories(budgetRequest);
            var signingStage = signatories.GetSigningStage();

            if ((signingStage >= SigningStage.Organization && budgetRequest.Requester.Adviser == user) ||
                (signingStage >= SigningStage.Deans &&
                 (user == signatories.AssistantDean.User || user == signatories.Dean.User)) ||
                (signingStage >= SigningStage.StudentAffairsDirector &&
                 user == signatories.StudentAffairsDirector.User))
            {
                budgetRequestsToShow.Add(budgetRequest);
            }
        }

        return budgetRequestsToShow;
    }

    public bool AddBudgetRequest(BudgetRequest budgetRequest, List<Expense> expenses)
    {
        BudgetRequests.Add(budgetRequest);
        Expenses.AddRange(expenses);
        AddSignatories(GetDefaultSignatories(budgetRequest));
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public bool RemoveBudgetRequest(BudgetRequest budgetRequest)
    {
        BudgetRequests.Remove(budgetRequest);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public List<Expense> GetExpenses(BudgetRequest budgetRequest)
    {
        return Expenses.Where(expense => expense.BudgetRequest == budgetRequest).ToList();
    }

    private Signatories GetDefaultSignatories(BudgetRequest budgetRequest)
    {
        var officerRoles = OfficerRoles.Where(officerRole => officerRole.Organization == budgetRequest.Requester);

        return new Signatories(
            Treasurer: new OfficerSignatory
            {
                BudgetRequest = budgetRequest,
                HasSigned = false,
                Role = officerRoles.FirstOrDefault(
                    officerRole => officerRole.Position == OrganizationPosition.Treasurer)
            },
            Auditor: new OfficerSignatory
            {
                BudgetRequest = budgetRequest,
                HasSigned = false,
                Role = officerRoles.FirstOrDefault(officerRole => officerRole.Position == OrganizationPosition.Auditor)
            },
            President: new OfficerSignatory
            {
                BudgetRequest = budgetRequest,
                HasSigned = false,
                Role = officerRoles.FirstOrDefault(
                    officerRole => officerRole.Position == OrganizationPosition.President)
            },
            Adviser: new AdminSignatory
            {
                BudgetRequest = budgetRequest,
                HasSigned = false,
                Role = AdminRoles.FirstOrDefault(officerRole =>
                    officerRole.Position == AdminPosition.Adviser &&
                    officerRole.Admin == budgetRequest.Requester.Adviser),
            },
            AssistantDean: new AdminSignatory
            {
                BudgetRequest = budgetRequest,
                HasSigned = false,
                Role = AdminRoles.FirstOrDefault(officerRole => officerRole.Position == AdminPosition.AssistantDean)
            },
            Dean: new AdminSignatory
            {
                BudgetRequest = budgetRequest,
                HasSigned = false,
                Role = AdminRoles.FirstOrDefault(officerRole => officerRole.Position == AdminPosition.Dean)
            },
            StudentAffairsDirector: new AdminSignatory
            {
                BudgetRequest = budgetRequest,
                HasSigned = false,
                Role = AdminRoles.FirstOrDefault(officerRole =>
                    officerRole.Position == AdminPosition.StudentAffairsDirector)
            });
    }

    private bool AddSignatories(Signatories signatories)
    {
        OfficerSignatories.Add(signatories.Treasurer);
        OfficerSignatories.Add(signatories.Auditor);
        OfficerSignatories.Add(signatories.President);
        AdminSignatories.Add(signatories.Adviser);
        AdminSignatories.Add(signatories.AssistantDean);
        AdminSignatories.Add(signatories.Dean);
        AdminSignatories.Add(signatories.StudentAffairsDirector);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }

    public List<AdminSignatory> GetAdminSignatories(BudgetRequest budgetRequest)
    {
        return AdminSignatories
            .Where(adminSignatory => adminSignatory.BudgetRequest == budgetRequest)
            .Include(adminSignatory => adminSignatory.Role)
            .Include(adminSignatory => adminSignatory.Role.Admin)
            .ToList();
    }

    public List<OfficerSignatory> GetOfficerSignatories(BudgetRequest budgetRequest)
    {
        return OfficerSignatories
            .Where(officerSignatory => officerSignatory.BudgetRequest == budgetRequest)
            .Include(officerRole => officerRole.Role)
            .Include(officerSignatory => officerSignatory.Role.Officer)
            .ToList();
    }

    public Signatories GetSignatories(BudgetRequest budgetRequest)
    {
        var officerSignatories = GetOfficerSignatories(budgetRequest);
        var adminSignatories = GetAdminSignatories(budgetRequest);

        var signatories = new Signatories(
            Treasurer: officerSignatories.First(signatory =>
                signatory.Role?.Position == OrganizationPosition.Treasurer),
            Auditor: officerSignatories.First(signatory =>
                signatory.Role?.Position == OrganizationPosition.Auditor),
            President: officerSignatories.First(signatory =>
                signatory.Role?.Position == OrganizationPosition.President),
            Adviser: adminSignatories.First(signatory =>
                signatory.Role?.Admin == budgetRequest.Requester.Adviser),
            AssistantDean: adminSignatories.First(signatory =>
                signatory.Role?.Position == AdminPosition.AssistantDean),
            Dean: adminSignatories.First(signatory =>
                signatory.Role?.Position == AdminPosition.Dean),
            StudentAffairsDirector: adminSignatories.First(signatory =>
                signatory.Role?.Position == AdminPosition.StudentAffairsDirector));
        return signatories;
    }

    public Signatory? GetSignatory(int id, bool isAdmin)
    {
        return isAdmin
            ? AdminSignatories
                .Where(adminSignatory => adminSignatory.Id == id)
                .Include(adminSignatory => adminSignatory.Role)
                .FirstOrDefault()
            : OfficerSignatories
                .Where(officerSignatory => officerSignatory.Id == id)
                .Include(officerSignatory => officerSignatory.Role)
                .FirstOrDefault();
    }

    public bool IsApproved(BudgetRequest budgetRequest)
    {
        var signatories = GetSignatories(budgetRequest);
        return signatories.ToList().All(signatory => signatory.HasSigned);
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

    private void CullOldTemporaryExpenses(int currentCreationId, User author)
    {
        var toRemove = TemporaryExpenses
            .Where(temporaryExpense => temporaryExpense.CreationId != currentCreationId 
                                       && temporaryExpense.Author == author);
        TemporaryExpenses.RemoveRange(toRemove);
        SaveChanges();
    }

    public void AddTemporaryExpense(TemporaryExpense temporaryExpense)
    {
        TemporaryExpenses.Add(temporaryExpense);
        SaveChanges();
    }

    public TemporaryExpense? GetTemporaryExpense(int id)
    {
        return TemporaryExpenses.FirstOrDefault(temporaryExpense => temporaryExpense.Id == id);
    }

    public List<TemporaryExpense> GetTemporaryExpenses(int currentCreationId, User author)
    {
        CullOldTemporaryExpenses(currentCreationId, author);
        return TemporaryExpenses.Where(temporaryExpense => temporaryExpense.CreationId == currentCreationId).ToList();
    }

    public void RemoveTemporaryExpense(TemporaryExpense temporaryExpense)
    {
        TemporaryExpenses.Remove(temporaryExpense);
    }
}