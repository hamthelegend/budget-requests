using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BudgetRequests.Models;
using BudgetRequests.Models.Organizations;

namespace BudgetRequests.Pages.OfficerRoles
{
    public class DeleteModel : PageModel
    {
        private readonly BudgetRequests.Models.DatabaseContext _context;

        public DeleteModel(BudgetRequests.Models.DatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
      public OfficerRole OfficerRole { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var officerRole = _context.GetOfficerRole((int)id);

            if (officerRole == null)
            {
                return NotFound();
            }
            else 
            {
                OfficerRole = officerRole;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var officerRole = _context.GetOfficerRole((int)id);

            if (officerRole != null)
            {
                OfficerRole = officerRole;
                _context.RemoveOfficerRole(OfficerRole);
            }

            return RedirectToPage("./Index");
        }
    }
}
