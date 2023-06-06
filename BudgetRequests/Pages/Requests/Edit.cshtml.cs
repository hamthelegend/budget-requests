using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BudgetRequests.Models;
using BudgetRequests.Models.BudgetRequests;

namespace BudgetRequests.Pages.Requests
{
    public class EditModel : PageModel
    {
        private readonly BudgetRequests.Models.DatabaseContext _context;

        public EditModel(BudgetRequests.Models.DatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BudgetRequest BudgetRequest { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var budgetRequest =  _context.GetBudgetRequest(id ?? -1);
            if (budgetRequest == null)
            {
                return NotFound();
            }
            BudgetRequest = budgetRequest;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(BudgetRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BudgetRequestExists(BudgetRequest.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BudgetRequestExists(int id)
        {
          return _context.GetBudgetRequest(id) != null;
        }
    }
}
