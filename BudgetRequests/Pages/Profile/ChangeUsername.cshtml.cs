using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BudgetRequests.Data;
using BudgetRequests.Models;
using BudgetRequests.Models.BudgetRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace BudgetRequests.Pages.Profile
{
    public class ChangeUsername : PageModel
    {
        private readonly BudgetRequests.Models.DatabaseContext _context;

        public ChangeUsername(BudgetRequests.Models.DatabaseContext context)
        {
            _context = context;
        }

        public new User User { get; set; }

        [BindProperty] public string? Username { get; set; }
        public string? UsernameError { get; set; }

        public IActionResult OnGet()
        {
            var user = HttpContext.Session.GetLoggedInUser(_context);

            if (user == null)
            {
                return RedirectToPage("../Login/Index");
            }
            
            User = user;
            
            return Page();
        }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPost()
        {
            OnGet();
            
            var hasError = false;
            
            if (Username.IsNullOrEmpty())
            {
                UsernameError = "Username cannot be blank";
                hasError = true;
            }

            if (hasError)
            {
                return Page();
            }

            if (!_context.IsUsernameAvailable(Username!))
            {
                UsernameError = "That username is already taken.";
                hasError = true;
            }

            if (hasError)
            {
                return Page();
            }

            User.Username = Username!;
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }

        public IActionResult OnPostCancel()
        {
            return RedirectToPage("./Index");
        }
    }
}