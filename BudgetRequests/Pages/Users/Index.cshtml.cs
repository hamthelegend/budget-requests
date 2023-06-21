using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetRequests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BudgetRequests.Models;

namespace BudgetRequests.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly BudgetRequests.Models.DatabaseContext _context;

        public IndexModel(BudgetRequests.Models.DatabaseContext context)
        {
            _context = context;
        }

        public List<User> Users { get; set; } = default!;
        public new User User { get; set; }
        public bool IsSuperAdmin { get; set; } = false;

        public IActionResult OnGetAsync()
        {
            var user = HttpContext.Session.GetLoggedInUser(_context);
            
            if (user == null)
            {
                return RedirectToPage("../Login/Index");
            }
            
            User = user;
            Users = _context.GetUsers();
            IsSuperAdmin = _context.IsSuperAdmin(user);

            return Page();
        }
    }
}