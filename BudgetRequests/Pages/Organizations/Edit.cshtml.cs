using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BudgetRequests.Models;
using BudgetRequests.Models.Organizations;

namespace BudgetRequests.Pages.Organizations;

public class EditModel : PageModel
{
    private readonly BudgetRequests.Models.DatabaseContext _context;

    public EditModel(BudgetRequests.Models.DatabaseContext context)
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

    public IActionResult OnGet(int? id)
    {
        var organization = _context.GetOrganization(id ?? -1);

        if (organization == null)
        {
            return NotFound();
        }

        Organization = organization;
        
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

        var organizationOfficers = _context.GetOrganizationOfficers(organization);

        PresidentId = organizationOfficers.President.Id.ToString();
        VicePresidentId = organizationOfficers.VicePresident.Id.ToString();
        SecretaryId = organizationOfficers.Secretary.Id.ToString();
        TreasurerId = organizationOfficers.Treasurer.Id.ToString();
        AuditorId = organizationOfficers.Auditor.Id.ToString();
        PublicRelationsOfficerId = organizationOfficers.PublicRelationsOfficer.Id.ToString();
        
        AdviserId = Organization.Adviser!.Id.ToString();

        ModelState.Clear();
        
        return Page();
    }


    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        Organization.Adviser = _context.GetUser(Convert.ToInt32(AdviserId));
            
        _context.AddOrganization(Organization);
        await _context.SaveChangesAsync();

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
}