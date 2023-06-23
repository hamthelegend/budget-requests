using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BudgetRequests.Models;
using BudgetRequests.Models.Organizations;

namespace BudgetRequests.Pages.Organizations
{
    public class DetailsModel : PageModel
    {
        private readonly BudgetRequests.Models.DatabaseContext _context;

        public DetailsModel(BudgetRequests.Models.DatabaseContext context)
        {
            _context = context;
        }

      public Organization Organization { get; set; } = default!;

      public OrganizationOfficers OfficerRoles { get; set; }

        public IActionResult OnGet(int? id)
        {
            var organization = _context.GetOrganization(id ?? -1);
            
            if (organization == null)
            {
                return NotFound();
            }

            Organization = organization;
            OfficerRoles = _context.GetOrganizationOfficers(organization);
            
            return Page();
        }
    }
}
