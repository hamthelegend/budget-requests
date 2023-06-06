using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BudgetRequests.Models;
using BudgetRequests.Models.Organizations;

namespace BudgetRequests.Pages.OfficerRoles
{
    public class CreateModel : PageModel
    {
        private readonly BudgetRequests.Models.DatabaseContext _context;

        public CreateModel(BudgetRequests.Models.DatabaseContext context)
        {
            _context = context;
        }

        public User Officer { get; set; }
        public IEnumerable<SelectListItem> Organizations { get; set; } = default!;
        
        [BindProperty]
        public string OrganizationId { get; set; }
        [BindProperty]
        public OrganizationPosition Position { get; set; }


        public IActionResult OnGet(int? id)
        {
            var user = _context.GetUser(id ?? -1);
            if (user == null)
            {
                return NotFound();
            }

            Officer = user;
            Organizations = _context.GetOrganizations().Select(organization =>
                new SelectListItem
                {
                    Value = organization.Id.ToString(),
                    Text = organization.Name
                });
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPost(int? id)
        {
            var user = _context.GetUser(id ?? -1);
            if (user == null)
            {
                return NotFound();
            }

            Officer = user;
            
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var officerRole = new OfficerRole
            {
                Officer = Officer,
                Organization = _context.GetOrganization(Convert.ToInt32(OrganizationId))!,
                Position = Position
            };
            _context.AddOfficerRole(officerRole);

            return RedirectToPage("./Index");
        }
    }
}