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
    public class IndexModel : PageModel
    {
        private readonly BudgetRequests.Models.DatabaseContext _context;

        public IndexModel(BudgetRequests.Models.DatabaseContext context)
        {
            _context = context;
        }

        public IList<OfficerRole> OfficerRoles { get; set; } = default!;

        [BindProperty]
        public User Officer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.GetUsers() == null)
            {
                return NotFound();
            }

            var user = _context.GetUser((int)id);
            if (user == null)
            {
                return NotFound();
            }

            Officer = user;
            OfficerRoles = _context.GetOfficerRoles(Officer);
            // var organization = OfficerRoles[0].Organization;
            return Page();
        }
    }
}