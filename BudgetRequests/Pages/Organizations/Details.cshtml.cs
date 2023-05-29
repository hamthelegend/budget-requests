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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.GetOrganizations() == null)
            {
                return NotFound();
            }

            var organization = _context.GetOrganization((int)id);
            if (organization == null)
            {
                return NotFound();
            }
            else 
            {
                Organization = organization;
            }
            return Page();
        }
    }
}
