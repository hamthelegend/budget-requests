using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BudgetRequests.Models;
using BudgetRequests.Models.Admins;
using BudgetRequests.Models.Organizations;

namespace BudgetRequests.Pages.AdminRoles
{
    public class DeleteModel : PageModel
    {
        private readonly BudgetRequests.Models.DatabaseContext _context;

        public DeleteModel(BudgetRequests.Models.DatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
      public AdminRole AdminRole { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminRole = _context.GetAdminRole((int)id);

            if (adminRole == null)
            {
                return NotFound();
            }
            else 
            {
                AdminRole = adminRole;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var adminRole = _context.GetAdminRole((int)id);

            if (adminRole != null)
            {
                AdminRole = adminRole;
                _context.RemoveAdminRole(AdminRole);
            }

            return RedirectToPage("./Index");
        }
    }
}
