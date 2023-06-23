using BudgetRequests.Data;
using BudgetRequests.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BudgetRequests.Pages.Profile
{
    public class OldIndexModel : PageModel
    {
        private readonly BudgetRequests.Models.DatabaseContext _context;
        
        private readonly IWebHostEnvironment webHostEnvironment;

        public OldIndexModel(BudgetRequests.Models.DatabaseContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }
        public User User { get; set; } = default!;
        
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string RepeatPassword { get; set; }
        public string? SignaturePath { get; set; }
        [BindProperty]
        public IFormFile? Signature { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = HttpContext.Session.GetLoggedInUser(_context);
            if (user == null)
            {
                return RedirectToPage("../Login/Index");
            }
            User = user;
            Username = user.Username;
            if (user.SignatureFilename != null)
            {
                SignaturePath = Path.Combine("images", user.SignatureFilename);
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var user = HttpContext.Session.GetLoggedInUser(_context);
            if (user == null)
            {
                return NotFound();
            }
            User = user;
            
            var isUsernameDuplicate = _context.GetUsers().Any(x => x.Username == Username && Username != user.Username);
            
            if (!ModelState.IsValid || isUsernameDuplicate || Password != RepeatPassword)
            {
                return Page();
            }
            
            var passwordSalt = Hash.GenerateSalt();
            var passwordHash = Password.ComputeHash(passwordSalt);
            
            var imageDirectory = Path.Combine(webHostEnvironment.WebRootPath, "images");

            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }

            var extension = Signature?.FileName.Split(".").Last() ?? "jpg";
            var fileName = $"{Guid.NewGuid().ToString()}.{extension}";
            var filePath = Path.Combine(imageDirectory, fileName);

            var stream = new FileStream(filePath, FileMode.Create);
            Signature?.CopyToAsync(stream);
            
            User.Username = Username;
            User.PasswordHash = passwordHash;
            User.PasswordSalt = Convert.ToBase64String(passwordSalt);
            User.SignatureFilename = fileName;

            _context.Attach(User).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(User.Id))
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

        private bool UserExists(int id)
        {
          return _context.GetUsers().Any(e => e.Id == id);
        }
    }
}
