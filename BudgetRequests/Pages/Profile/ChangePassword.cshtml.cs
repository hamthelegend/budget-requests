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
    public class ChangePassword : PageModel
    {
        private readonly BudgetRequests.Models.DatabaseContext _context;

        public ChangePassword(BudgetRequests.Models.DatabaseContext context)
        {
            _context = context;
        }

        public new User User { get; set; }

        [BindProperty] public string? Password { get; set; }
        public string? PasswordError { get; set; }        
        
        [BindProperty] public string? RepeatPassword { get; set; }
        public string? RepeatPasswordError { get; set; }

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
            
            if (Password.IsNullOrEmpty())
            {
                PasswordError = "Password cannot be blank";
                hasError = true;
            }
            
            if (RepeatPassword.IsNullOrEmpty())
            {
                RepeatPasswordError = "Repeat Password is required";
                hasError = true;
            }

            if (hasError)
            {
                return Page();
            }

            if (Password != RepeatPassword)
            {
                RepeatPasswordError = "Passwords do not match";
                hasError = true;
            }

            if (hasError)
            {
                return Page();
            }

            var salt = Hash.GenerateSalt();
            var passwordHash = Password!.ComputeHash(salt);

            User.PasswordHash = passwordHash;
            User.PasswordSalt = Convert.ToBase64String(salt);
            
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }

        public IActionResult OnPostCancel()
        {
            return RedirectToPage("./Index");
        }
    }
}