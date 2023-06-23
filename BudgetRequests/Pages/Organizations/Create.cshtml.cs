using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BudgetRequests.Models;
using BudgetRequests.Models.Organizations;

namespace BudgetRequests.Pages.Organizations;

public class CreateModel : PageModel
{
    private readonly BudgetRequests.Models.DatabaseContext _context;

    public CreateModel(BudgetRequests.Models.DatabaseContext context)
    {
        _context = context;
    }

    public IEnumerable<SelectListItem> Admins { get; set; } = default!;
    public IEnumerable<SelectListItem> Officers { get; set; } = default!;

    [BindProperty] public Organization Organization { get; set; } = default!;

    [BindProperty] public string AdviserId { get; set; }
    [BindProperty] public string PresidentId { get; set; }
    [BindProperty] public string VicePresidentId { get; set; }
    [BindProperty] public string SecretaryId { get; set; }
    [BindProperty] public string TreasurerId { get; set; }
    [BindProperty] public string AuditorId { get; set; }
    [BindProperty] public string PublicRelationsOfficerId { get; set; }

    public IActionResult OnGet()
    {
        Admins = _context.GetAdmins().Select(admin =>
            new SelectListItem
            {
                Value = admin.Id.ToString(),
                Text = $"{admin.FirstName} {admin.LastName}"
            });
        Officers = _context.GetOfficers().Select(officer =>
            new SelectListItem
            {
                Value = officer.Id.ToString(),
                Text = $"{officer.FirstName} {officer.LastName}"
            });
        return Page();
    }


    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public IActionResult OnPostAdd()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        Organization.Adviser = _context.GetUser(Convert.ToInt32(AdviserId));
            
        _context.AddOrganization(Organization);
        _context.SaveChanges();

        var president = _context.GetOfficer(Convert.ToInt32(PresidentId));
        var vicePresident = _context.GetOfficer(Convert.ToInt32(VicePresidentId));
        var secretary = _context.GetOfficer(Convert.ToInt32(SecretaryId));
        var treasurer = _context.GetOfficer(Convert.ToInt32(TreasurerId));
        var auditor = _context.GetOfficer(Convert.ToInt32(AuditorId));
        var publicRelationsOfficer = _context.GetOfficer(Convert.ToInt32(PublicRelationsOfficerId));

        var organizationOfficers = new OrganizationOfficers(
            Organization, 
            president, 
            vicePresident, 
            secretary,
            treasurer, 
            auditor, 
            publicRelationsOfficer);

        _context.SetOrganizationOfficers(organizationOfficers);
        
        return RedirectToPage("./Index");
    }

    public IActionResult OnPostCancel()
    {
        return RedirectToPage("./Index");
    }
    
}