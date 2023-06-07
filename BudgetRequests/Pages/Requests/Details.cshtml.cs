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
        private readonly BudgetRequests.Models.DatabaseContext _context;

        public DetailsModel(BudgetRequests.Models.DatabaseContext context)
        {
            _context = context;
        }

        public User User { get; set; } = default!;

        public BudgetRequest BudgetRequest { get; set; } = default!;

        public Signatories Signatories { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var user = HttpContext.Session.GetLoggedInUser(_context);
            var budgetRequest = _context.GetBudgetRequest(id ?? -1);

            if (user == null || budgetRequest == null)
            {
                return NotFound();
            }

            User = user;
            BudgetRequest = budgetRequest;
            Signatories = _context.GetSignatories(budgetRequest);

            return Page();
        }

        public IActionResult OnPostSign(int? id)
        {
            var user = HttpContext.Session.GetLoggedInUser(_context);
            var budgetRequest = _context.GetBudgetRequest(id ?? -1);

            if (user == null || budgetRequest == null)
            {
                return NotFound();
            }

            User = user;
            BudgetRequest = budgetRequest;
            Signatories = _context.GetSignatories(budgetRequest);
            
            var signatory = Signatories.ToList().First(signatory => signatory.User == User);
            signatory.HasSigned = true;
            _context.SaveChanges();
            return Page();
        }

        public IActionResult OnPostUnsign(int? id)
        {
            var user = HttpContext.Session.GetLoggedInUser(_context);
            var budgetRequest = _context.GetBudgetRequest(id ?? -1);

            if (user == null || budgetRequest == null)
            {
                return NotFound();
            }

            User = user;
            BudgetRequest = budgetRequest;
            Signatories = _context.GetSignatories(budgetRequest);
            
            var signatory = Signatories.ToList().First(signatory => signatory.User == User);
            signatory.HasSigned = false;
            _context.SaveChanges();
            return Page();
        }

        // public void OnPostApprove(int? signatoryIndex)
        // {
        //     
        // }
    }
}