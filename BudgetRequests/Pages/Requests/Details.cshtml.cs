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
    public class DetailsModel : PageModel
    {
        private readonly DatabaseContext _context;

        public DetailsModel(DatabaseContext context)
        {
            _context = context;
        }

        public new User User { get; set; } = default!;

        public BudgetRequest BudgetRequest { get; set; } = default!;

        public List<Expense> Expenses { get; set; } = new();

        public Signatories Signatories { get; set; } = default!;
        
        public bool CanUserDeleteBudgetRequest { get; set; }

        public IActionResult OnGet(int? id)
        {
            var user = HttpContext.Session.GetLoggedInUser(_context);
            var budgetRequest = _context.GetBudgetRequest(id ?? -1);

            if (user == null || budgetRequest == null)
            {
                return NotFound();
            }

            User = user;
            BudgetRequest = budgetRequest;
            Expenses = _context.GetExpenses(budgetRequest);
            Signatories = _context.GetSignatories(budgetRequest);
            CanUserDeleteBudgetRequest = _context.CanUserDeleteBudgetRequest(user, budgetRequest);

            return Page();
        }

        public IActionResult OnPostSign(int? budgetRequestId, int? signatoryId, bool isAdmin)
        {
            OnGet(budgetRequestId);
            
            var signatory = _context.GetSignatory(signatoryId ?? -1, isAdmin);

            if (signatory != null)
            {
                signatory.HasSigned = true;
            }
            
            _context.SaveChanges();
            return Page();
        }

        public IActionResult OnPostUnsign(int? budgetRequestId, int? signatoryId, bool isAdmin)
        {
            OnGet(budgetRequestId);
            
            var signatory = _context.GetSignatory(signatoryId ?? -1, isAdmin);

            if (signatory != null)
            {
                signatory.HasSigned = false;
            }
            
            _context.SaveChanges();
            return Page();
        }

        public IActionResult OnPostComments(int id)
        {
            return RedirectToPage("./Comments", new { id });
        }

        public IActionResult OnPostDelete(int id)
        {
            var budgetRequest = _context.GetBudgetRequest(id);
            
            if (budgetRequest != null)
            {
                _context.RemoveBudgetRequest(budgetRequest);
            }

            return RedirectToPage("./Index");
        }

        public IActionResult OnPostPrint(int id)
        {
            return RedirectToPage("../PrintRequest/Index", new { id });
        }

        public IActionResult OnPostBack()
        {
            return RedirectToPage("./Index");
        }
    }
}