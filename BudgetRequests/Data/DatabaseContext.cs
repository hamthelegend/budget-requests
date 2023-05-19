using BudgetRequests.DatabaseModels.Admins;
using BudgetRequests.DatabaseModels.BudgetRequests;
using BudgetRequests.DatabaseModels.Organizations;
using BudgetRequests.DatabaseModels.Users;
using BudgetRequests.DomainModels;
using BudgetRequests.DomainModels.Admins;
using BudgetRequests.DomainModels.Organizations;
using BudgetRequests.DomainModels.Users;
using Microsoft.EntityFrameworkCore;

namespace BudgetRequests.Models;

public class DatabaseContext : DbContext
{
    public const string ConnectionString =
        @"Server=(localdb)\mssqllocaldb;Database=BudgetRequests;Trusted_Connection=True";

    public const int NoId = -1;

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    private DbSet<DbUser> Users { get; set; }
    private DbSet<DbAdminRole> AdminRoles { get; set; }
    private DbSet<DbOfficerRole> OfficerRoles { get; set; }
    private DbSet<DbOrganization> Organizations { get; set; }
    private DbSet<DbPreference> Preferences { get; set; }

    private DbSet<DbBudgetRequest> BudgetRequests { get; set; }
    private DbSet<DbExpense> Expenses { get; set; }
    private DbSet<DbAdminSignatory> AdminSignatories { get; set; }
    private DbSet<DbOfficerSignatory> OfficerSignatories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionString);
    }

    private Admin ToAdmin(DbUser user)
    {
        var positions = AdminRoles
            .Where(role => role.Admin == user)
            .Select(role => role.Position)
            .ToList();
        return new Admin(
            Id: user.Id,
            FirstName: user.FirstName,
            MiddleName: user.MiddleName,
            LastName: user.LastName,
            Username: user.Username,
            PasswordHash: user.PasswordHash,
            PasswordSalt: user.PasswordSalt,
            Positions: positions);
    }

    private Officer ToOfficer(DbUser user)
    {
        var roles = OfficerRoles
            .Where(officerRole => officerRole.Officer == user)
            .ToList()
            .Select(dbRole =>
                new OfficerRole(
                    Organization: new Organization(
                        Id: dbRole.Organization.Id,
                        Name: dbRole.Organization.Name,
                        Adviser: ToAdmin(dbRole.Organization.Adviser!),
                        IsStudentCouncil: dbRole.Organization.IsStudentCouncil,
                        GrossBudget: dbRole.Organization.GrossBudget),
                    dbRole.Position))
            .ToList();
        return new Officer(
            Id: user.Id,
            FirstName: user.FirstName,
            MiddleName: user.MiddleName,
            LastName: user.LastName,
            Username: user.Username,
            PasswordHash: user.PasswordHash,
            PasswordSalt: user.PasswordSalt,
            Roles: roles);
    }

    private User ToUser(DbUser user)
    {
        return user.Type == UserType.Admin ? ToAdmin(user) : ToOfficer(user);
    }

    private DbUser ToDbUser(User user)
    {
        if (user.Id == NoId)
        {
            return new DbUser
            {
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Username = user.Username,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt,
                Type = user is Admin ? UserType.Admin : UserType.Officer
            };
        }

        return new DbUser
        {
            Id = user.Id,
            FirstName = user.FirstName,
            MiddleName = user.MiddleName,
            LastName = user.LastName,
            Username = user.Username,
            PasswordHash = user.PasswordHash,
            PasswordSalt = user.PasswordSalt,
            Type = UserType.Admin
        };
    }

    private DbAdminRole ToDbAdminRole(DbUser admin, AdminPosition position)
    {
        return new DbAdminRole
        {
            Admin = admin,
            Position = position
        };
    }

    private DbOrganization ToDbOrganization(Organization organization)
    {
        var adviser = Users.FirstOrDefault(dbUser => organization.Adviser.Id == dbUser.Id);
        
        if (organization.Id == NoId)
        {

            return new DbOrganization
            {
                Name = organization.Name,
                Adviser = adviser,
                GrossBudget = organization.GrossBudget,
                IsStudentCouncil = organization.IsStudentCouncil
            };
        }
        
        return new DbOrganization
        {
            Id = organization.Id,
            Name = organization.Name,
            Adviser = adviser,
            GrossBudget = organization.GrossBudget,
            IsStudentCouncil = organization.IsStudentCouncil
        };
    }

    public IEnumerable<User> GetUsers()
    {
        return Users.Select(user => ToUser(user)).ToList();
    }

    public User? GetUser(int id)
    {
        var user = Users.FirstOrDefault(user => user.Id == id);
        return user != null ? ToUser(user) : null;
    }

    public void AddUpdateUser(User user)
    {
        var dbUser = ToDbUser(user);
        if (user.Id == NoId)
        {
            Users.Add(dbUser);
        }
        else
        {
            Users.Update(dbUser);
        }

        if (user is Admin admin)
        {
            var existingRoles = AdminRoles.Where(role => role.Admin.Id == dbUser.Id);
            AdminRoles.RemoveRange(existingRoles);

            foreach (var position in admin.Positions)
            {
                var dbRole = ToDbAdminRole(dbUser, position);
                AdminRoles.Add(dbRole);
            }
        }
        else if (user is Officer officer)
        {
            var existingRoles = OfficerRoles.Where(role => role.Officer.Id == dbUser.Id);
            OfficerRoles.RemoveRange(existingRoles);
            
            foreach (var role in officer.Roles)
            {
                var organization =
                    Organizations.FirstOrDefault(dbOrganization => dbOrganization.Id == role.Organization.Id);
                if (organization == null) continue;
                var dbRole = new DbOfficerRole
                {
                    Officer = dbUser,
                    Organization = organization,
                    Position = role.Position
                };
                OfficerRoles.Add(dbRole);
            }
        }

        SaveChanges();
    }

    public void AddUpdateOrganization(Organization organization)
    {
        var dbOrganization = ToDbOrganization(organization);
        if (organization.Id == NoId)
        {
            Organizations.Add(dbOrganization);
        }
        else
        {
            Organizations.Update(dbOrganization);
        }
    }

    public bool HasSuperAdmin()
    {
        return AdminRoles.Any(role => role.Position == AdminPosition.SuperAdmin);
    }

    public DbPreference? GetPreference()
    {
        return Preferences.LastOrDefault();
    }

    public bool SetPreference(DbPreference preference)
    {
        Preferences.Add(preference);
        var changesSaved = SaveChanges();
        return changesSaved > 0;
    }
}