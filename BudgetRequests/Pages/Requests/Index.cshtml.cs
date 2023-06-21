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
using BudgetRequests.Models.Organizations;

namespace BudgetRequests.Pages.Requests
{
    public class IndexModel : PageModel
    {
        private readonly BudgetRequests.Models.DatabaseContext _context;

        public IndexModel(BudgetRequests.Models.DatabaseContext context)
        {
            _context = context;
        }

        public IList<BudgetRequest> BudgetRequests { get; set; } = default!;
        public List<bool> IsApproved { get; set; } = new();

        public new User User { get; set; }

        public bool CanCreateRequests { get; set; }
        
        public bool AreCollegeAdminsSet { get; set; }

        public IActionResult OnGet()
        {
            var user = HttpContext.Session.GetLoggedInUser(_context);
            if (user == null)
            {
                return RedirectToPage("../Login/Index");
            }

            User = user;
            BudgetRequests = _context.GetBudgetRequests(user);
            CanCreateRequests = _context.CanCreateRequests(user);
            AreCollegeAdminsSet = _context.AreCollegeAdminsSet();
                        
            foreach (var budgetRequest in BudgetRequests)
            {
                IsApproved.Add(_context.IsApproved(budgetRequest));
            }

            return Page();
        }

        public string GetFormattedTotalExpenses(BudgetRequest budgetRequest)
        {
            var expenses = _context.GetExpenses(budgetRequest);
            var totalExpenses = expenses.Sum(expense => expense.Amount);
            return $"₱ {totalExpenses.ToString("0.00")}";
        }
    }
}