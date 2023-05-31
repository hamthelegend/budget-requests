using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BudgetRequests.Models;
using BudgetRequests.Models.Organizations;

namespace BudgetRequests.Pages.Organizations
{
    public class CreateModel : PageModel
    {
        private readonly BudgetRequests.Models.DatabaseContext _context;

        public CreateModel(BudgetRequests.Models.DatabaseContext context)
        {
            _context = context;
        }
        
        public IEnumerable<SelectListItem> Admins { get; set; } = default!;

        [BindProperty]
        public Organization Organization { get; set; } = default!;
        
        [BindProperty] public string AdviserId { get; set; }

        public IActionResult OnGet()
        { 
            Admins = _context.GetAdmins().Select(admin =>
                new SelectListItem
                {
                    Value = admin.Id.ToString(),
                    Text = $"{admin.FirstName} {admin.LastName}"
                });
            return Page();
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.GetOrganizations() == null || Organization == null)
            {
                return Page();
            }

          Organization.Adviser = _context.GetUser(Convert.ToInt32(AdviserId));

            _context.AddOrganization(Organization);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
