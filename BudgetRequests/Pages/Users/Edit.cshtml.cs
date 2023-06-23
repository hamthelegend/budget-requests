using BudgetRequests.Data;
using BudgetRequests.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace BudgetRequests.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly DatabaseContext _context;

        public EditModel(DatabaseContext context)
        {
            _context = context;
        }

        public new User User { get; set; }
        
        [BindProperty]
        public string? FirstName { get; set; }
        public string? FirstNameError { get; set; }

        [BindProperty]
        public string? MiddleName { get; set; }
        public string? MiddleNameError { get; set; }

        [BindProperty]
        public string? LastName { get; set; }
        public string? LastNameError { get; set; }

        [BindProperty]
        public string? Username { get; set; }
        public string? UsernameError { get; set; }

        [BindProperty]
        public string? Password { get; set; }
        public string? PasswordError { get; set; }
        
        [BindProperty]
        public string? RepeatPassword { get; set; }
        public string? RepeatPasswordError { get; set; }

        public IActionResult OnGet()
        {
            var user = HttpContext.Session.GetLoggedInUser(_context);

            if (user == null)
            {
                return RedirectToPage("../Login/Index");
            }
            
            User = user;

            FirstName = user.FirstName;
            MiddleName = user.MiddleName;
            LastName = user.LastName;
            Username = user.Username;

            return Page();
        }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPostSave()
        {
            OnGet();
            
            var hasError = false;
            
            if (FirstName.IsNullOrEmpty())
            {
                FirstNameError = "First name is required";
                hasError = true;
            }
            
            if (LastName.IsNullOrEmpty())
            {
                LastNameError = "Last name is required";
                hasError = true;
            }
            
            if (Username.IsNullOrEmpty())
            {
                UsernameError = "Username is required";
                hasError = true;
            }
            
            if (Password.IsNullOrEmpty())
            {
                PasswordError = "Password is required";
                hasError = true;
            }
            
            if (RepeatPassword.IsNullOrEmpty())
            {
                RepeatPasswordError = "Repeat password is required";
                hasError = true;
            }

            if (hasError)
            {
                return Page();
            }
            
            if (!_context.IsUsernameAvailable(Username!) && User.Username != Username)
            {
                UsernameError = "That username is already taken";
                hasError = true;
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

            User.FirstName = FirstName!;
            User.MiddleName = MiddleName != "" ? MiddleName : null;
            User.LastName = LastName!;
            User.Username = Username!;

            var salt = Hash.GenerateSalt();
            var passwordHash = Password!.ComputeHash(salt);

            User.PasswordHash = passwordHash;
            User.PasswordSalt = Convert.ToBase64String(salt);
            
            _context.SaveChanges();

            return RedirectToPage("./Details");
        }

        public IActionResult OnPostCancel(int id)
        {
            OnGet();
            return RedirectToPage("./Details", new { User.Id });
        }
    }
}