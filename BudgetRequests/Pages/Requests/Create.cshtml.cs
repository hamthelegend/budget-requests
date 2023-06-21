using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BudgetRequests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BudgetRequests.Models;
using BudgetRequests.Models.BudgetRequests;
using BudgetRequests.Models.Organizations;
using Microsoft.IdentityModel.Tokens;

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
        
        public string? GlobalError { get; set; }

        [BindProperty] public string? Title { get; set; }
        public string? TitleError { get; set; }

        [BindProperty] public string? Body { get; set; }
        public string? BodyError { get; set; }

        [BindProperty] public string? OrganizationId { get; set; }
        public string? OrganizationError { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date needed")]
        [BindProperty]
        public DateTime? DateNeeded { get; set; }

        public string? DateNeededError { get; set; }

        public IActionResult OnGet()
        {
            var user = HttpContext.Session.GetLoggedInUser(_context);

            if (user == null)
            {
                return RedirectToPage("../Login/Index");
            }

            var canCreateRequests = _context.CanCreateRequests(user);

            if (!canCreateRequests)
            {
                return RedirectToPage("./Index");
            }

            Organizations = _context.GetOfficerOrganizations(user).Select(organization =>
                new SelectListItem
                {
                    Value = organization.Id.ToString(),
                    Text = organization.Name
                });
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPost()
        {
            OnGet();
            
            var hasError = false;
            
            if (Title.IsNullOrEmpty())
            {
                TitleError = "Title is required";
                hasError = true;
            }
            
            if (Body.IsNullOrEmpty())
            {
                BodyError = "Body is required";
                hasError = true;
            }
            
            if (OrganizationId.IsNullOrEmpty())
            {
                OrganizationError = "Requester is required";
                hasError = true;
            }
            
            if (DateNeeded == null)
            {
                DateNeededError = "Date needed is required";
                hasError = true;
            }

            if (hasError)
            {
                return Page();
            }
            
            if (DateNeeded <= DateTime.Now.Date)
            {
                DateNeededError = "Date needed should be in the future";
                hasError = true;
            }

            if (hasError)
            {
                return Page();
            }

            _context.AddBudgetRequest(new BudgetRequest
            {
                Title = Title!,
                Body = Body!,
                Requester = _context.GetOrganization(Convert.ToInt32(OrganizationId))!,
                DateNeeded = (DateTime) DateNeeded!
            }, new List<Expense>());

            return RedirectToPage("./Index");
        }
    }
}