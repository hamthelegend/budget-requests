using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BudgetRequests.Models;
using BudgetRequests.Models.BudgetRequests;

namespace BudgetRequests.Pages.Requests
{
    public class DeleteModel : PageModel
    {
        private readonly BudgetRequests.Models.DatabaseContext _context;

        public DeleteModel(BudgetRequests.Models.DatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
      public BudgetRequest BudgetRequest { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var budgetRequest = _context.GetBudgetRequest(id ?? -1);

            if (budgetRequest == null)
            {
                return NotFound();
            }
            else 
            {
                BudgetRequest = budgetRequest;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var budgetRequest = _context.GetBudgetRequest(id ?? -1);

            if (budgetRequest != null)
            {
                BudgetRequest = budgetRequest;
                _context.RemoveBudgetRequest(BudgetRequest);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
