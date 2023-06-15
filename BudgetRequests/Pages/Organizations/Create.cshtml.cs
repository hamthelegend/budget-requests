using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BudgetRequests.Models;
using BudgetRequests.Models.Organizations;

namespace BudgetRequests.Pages.Organizations
{
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

            _context.AddOfficerRole(new OfficerRole
            {
                Officer = president!,
                Organization = Organization,
                Position = OrganizationPosition.President
            });
            _context.AddOfficerRole(new OfficerRole
            {
                Officer = vicePresident!,
                Organization = Organization,
                Position = OrganizationPosition.VicePresident
            });
            _context.AddOfficerRole(new OfficerRole
            {
                Officer = secretary!,
                Organization = Organization,
                Position = OrganizationPosition.Secretary
            });
            _context.AddOfficerRole(new OfficerRole
            {
                Officer = treasurer!,
                Organization = Organization,
                Position = OrganizationPosition.Treasurer
            });
            _context.AddOfficerRole(new OfficerRole
            {
                Officer = auditor!,
                Organization = Organization,
                Position = OrganizationPosition.Auditor
            });
            _context.AddOfficerRole(new OfficerRole
            {
                Officer = publicRelationsOfficer!,
                Organization = Organization,
                Position = OrganizationPosition.PublicRelationsOfficer
            });

            return RedirectToPage("./Index");
        }
    }
}