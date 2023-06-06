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
    public class DetailsModel : PageModel
    {
        private readonly BudgetRequests.Models.DatabaseContext _context;

        public DetailsModel(BudgetRequests.Models.DatabaseContext context)
        {
            _context = context;
        }

      public User User { get; set; } = default!; 

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
            else 
            {
                User = user;
            }
            return Page();
        }
    }
}
