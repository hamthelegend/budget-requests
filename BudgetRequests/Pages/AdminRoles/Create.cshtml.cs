using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BudgetRequests.Models;
using BudgetRequests.Models.Admins;
using BudgetRequests.Models.Organizations;

namespace BudgetRequests.Pages.AdminRoles
{
    public class CreateModel : PageModel
    {
        private readonly BudgetRequests.Models.DatabaseContext _context;

        public CreateModel(BudgetRequests.Models.DatabaseContext context)
        {
            _context = context;
        }

        public User Admin { get; set; }
        [BindProperty]
        public AdminPosition Position { get; set; }


        public IActionResult OnGet(int? id)
        {
            var user = _context.GetUser(id ?? -1);
            if (user == null)
            {
                return NotFound();
            }

            Admin = user;
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPost(int? id)
        {
            var user = _context.GetUser(id ?? -1);
            if (user == null)
            {
                return NotFound();
            }

            Admin = user;
            
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var adminRole = new AdminRole
            {
                Admin = Admin,
                Position = Position
            };
            _context.AddAdminRole(adminRole);

            return RedirectToPage("./Index");
        }
    }
}