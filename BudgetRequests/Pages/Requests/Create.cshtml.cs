using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetRequests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BudgetRequests.Models;
using BudgetRequests.Models.BudgetRequests;
using BudgetRequests.Models.Organizations;

namespace BudgetRequests.Pages.Requests
{
    public class CreateModel : PageModel
    {
        private readonly BudgetRequests.Models.DatabaseContext _context;

        public CreateModel(BudgetRequests.Models.DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> Organizations { get; set; } = default!;

        [BindProperty] public string Title { get; set; }

        [BindProperty] public string Body { get; set; }

        [BindProperty] public string OrganizationId { get; set; }

        public IActionResult OnGet()
        {
            var officer = HttpContext.Session.GetLoggedInUser(_context)!; // TODO: Remove !
            Organizations = _context.GetOfficerOrganizations(officer).Select(organization =>
                new SelectListItem
                {
                    Value = organization.Id.ToString(),
                    Text = organization.Name
                });
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.AddBudgetRequest(new BudgetRequest
            {
                Title = Title,
                Body = Body,
                Requester = _context.GetOrganization(Convert.ToInt32(OrganizationId))!
            }, new List<Expense>());

            return RedirectToPage("./Index");
        }
    }
}