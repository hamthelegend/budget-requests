using BudgetRequests.Data;
using BudgetRequests.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BudgetRequests.Pages.Profile
{
    public class ChangeSignatureModel : PageModel
    {
        private readonly BudgetRequests.Models.DatabaseContext _context;
        
        private readonly IWebHostEnvironment webHostEnvironment;

        public ChangeSignatureModel(BudgetRequests.Models.DatabaseContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }
        public User User { get; set; } = default!;
        public string? SignaturePath { get; set; }
        [BindProperty]
        public IFormFile? Signature { get; set; }
        public string? SignatureError { get; set; }

        public IActionResult OnGet()
        {
            var user = HttpContext.Session.GetLoggedInUser(_context);
            if (user == null)
            {
                return RedirectToPage("../Login/Index");
            }
            User = user;
            if (user.SignatureFilename != null)
            {
                SignaturePath = Path.Combine("/images", user.SignatureFilename);
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnPostSave()
        {
            OnGet();

            var imageDirectory = Path.Combine(webHostEnvironment.WebRootPath, "images");

            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }

            var hasError = false;

            var extension = Signature?.FileName.Split(".").Last() ?? "jpg";
            var fileName = $"{Guid.NewGuid().ToString()}.{extension}";
            var filePath = Path.Combine(imageDirectory, fileName);

            if (Signature == null)
            {
                SignatureError = "Please upload a signature";
                hasError = true;
            }

            if (hasError)
            {
                return Page();
            }
            
            var stream = new FileStream(filePath, FileMode.Create);
            Signature?.CopyToAsync(stream);
            
            User.SignatureFilename = fileName;
            
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }

        public IActionResult OnPostCancel()
        {
            return RedirectToPage("./Index");
        }
    }
}
