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
    public class DetailsModel : PageModel
    {
        private readonly BudgetRequests.Models.DatabaseContext _context;

        public DetailsModel(BudgetRequests.Models.DatabaseContext context)
        {
            _context = context;
        }

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

            AdminRole = adminRole;
            return Page();
        }
    }
}
