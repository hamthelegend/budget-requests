using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetRequests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BudgetRequests.Models;
using BudgetRequests.Models.Organizations;

namespace BudgetRequests.Pages.Organizations
{
    public class IndexModel : PageModel
    {
        private readonly BudgetRequests.Models.DatabaseContext _context;

        public IndexModel(BudgetRequests.Models.DatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<Organization> Organizations { get; set; } = new();

        public new User User { get; set; } = new();
        public bool IsSuperAdmin { get; set; } = false;

        public IActionResult OnGet()
        {
            var user = HttpContext.Session.GetLoggedInUser(_context);
            
            if (user == null)
            {
                return RedirectToPage("../Login/Index");
            }

            User = user;
            Organizations = _context.GetOrganizations();
            IsSuperAdmin = _context.IsSuperAdmin(user);
            
            return Page();
        }
    }
}