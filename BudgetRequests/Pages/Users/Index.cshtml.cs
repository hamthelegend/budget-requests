using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public List<User> User { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.GetUsers() != null)
            {
                User = _context.GetUsers();
            }
        }
    }
}
