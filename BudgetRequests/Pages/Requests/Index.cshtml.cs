using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetRequests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BudgetRequests.Models;
using BudgetRequests.Models.BudgetRequests;

namespace BudgetRequests.Pages.Requests
{
    public class IndexModel : PageModel
    {
        private readonly BudgetRequests.Models.DatabaseContext _context;

        public IndexModel(BudgetRequests.Models.DatabaseContext context)
        {
            _context = context;
        }

        public IList<BudgetRequest> BudgetRequest { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var user = HttpContext.Session.GetLoggedInUser(_context);
            if (user == null)
            {
                return;
            }
            BudgetRequest = _context.GetBudgetRequests(user);
        }
    }
}