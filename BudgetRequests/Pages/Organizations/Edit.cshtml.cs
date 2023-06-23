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
using Microsoft.IdentityModel.Tokens;

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
    public string NameError { get; set; }

    [BindProperty] public string? AdviserId { get; set; }
    public string? AdviserError { get; set; }
    
    [BindProperty] public string? PresidentId { get; set; }
    public string? PresidentError { get; set; }

    [BindProperty] public string? VicePresidentId { get; set; }
    public string? VicePresidentError { get; set; }

    [BindProperty] public string? SecretaryId { get; set; }
    public string? SecretaryError { get; set; }

    [BindProperty] public string? TreasurerId { get; set; }
    public string? TreasurerError { get; set; }

    [BindProperty] public string? AuditorId { get; set; }
    public string? AuditorError { get; set; }

    [BindProperty] public string? PublicRelationsOfficerId { get; set; }
    public string? PublicRelationsOfficerError { get; set; }

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

        PresidentId = organizationOfficers.President.Officer.Id.ToString();
        VicePresidentId = organizationOfficers.VicePresident.Officer.Id.ToString();
        SecretaryId = organizationOfficers.Secretary.Officer.Id.ToString();
        TreasurerId = organizationOfficers.Treasurer.Officer.Id.ToString();
        AuditorId = organizationOfficers.Auditor.Officer.Id.ToString();
        PublicRelationsOfficerId = organizationOfficers.PublicRelationsOfficer.Officer.Id.ToString();
        
        AdviserId = Organization.Adviser!.Id.ToString();

        ModelState.Clear();
        
        return Page();
    }


    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public IActionResult OnPostSave(int id)
    {
        var organization = _context.GetOrganization(id);

        if (organization == null)
        {
            return NotFound();
        }

        Organization = organization;
        
        var hasError = false;

        if (Organization.Name.IsNullOrEmpty())
        {
            NameError = "Name is required";
            hasError = true;
        }

        Organization.Adviser = _context.GetUser(Convert.ToInt32(AdviserId));
        
        if (Organization.Adviser == null)
        {
            AdviserError = "Adviser is required";
            hasError = true;
        }
        
        _context.SaveChanges();

        var president = _context.GetOfficer(Convert.ToInt32(PresidentId));
        var vicePresident = _context.GetOfficer(Convert.ToInt32(VicePresidentId));
        var secretary = _context.GetOfficer(Convert.ToInt32(SecretaryId));
        var treasurer = _context.GetOfficer(Convert.ToInt32(TreasurerId));
        var auditor = _context.GetOfficer(Convert.ToInt32(AuditorId));
        var publicRelationsOfficer = _context.GetOfficer(Convert.ToInt32(PublicRelationsOfficerId));
        
        if (president == null)
        {
            PresidentError = "President is required";
            hasError = true;
        }
        
        if (vicePresident == null)
        {
            VicePresidentError = "Vice President is required";
            hasError = true;
        }
        
        if (secretary == null)
        {
            SecretaryError = "Secretary is required";
            hasError = true;
        }
        
        if (treasurer == null)
        {
            TreasurerError = "Treasurer is required";
            hasError = true;
        }
        
        if (auditor == null)
        {
            AuditorError = "Auditor is required";
            hasError = true;
        }
        
        if (publicRelationsOfficer == null)
        {
            PublicRelationsOfficerError = "P.R.O. is required";
            hasError = true;
        }

        if (hasError)
        {
            return Page();
        }

        var organizationOfficers = new OrganizationOfficers(
            Organization, 
            president!, 
            vicePresident!, 
            secretary!,
            treasurer!, 
            auditor!, 
            publicRelationsOfficer!);

        _context.SetOrganizationOfficers(organizationOfficers);

        return RedirectToPage("./Details", new { id });
    }
    
    public IActionResult OnPostCancel(int id)
    {
        OnGet(id);
        return RedirectToPage("./Details", new { id });
    }
}