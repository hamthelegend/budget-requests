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
        
        public int CreationId { get; set; }
        
        public new User User { get; set; }

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
        
        [BindProperty] public string? Purpose { get; set; }
        public string? PurposeError { get; set; }
        [BindProperty] public decimal? Amount { get; set; }
        public string? AmountError { get; set; }
        
        public List<TemporaryExpense> TemporaryExpenses { get; set; }

        public IActionResult OnGet(int creationId)
        {
            CreationId = creationId;
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

            User = user;
            TemporaryExpenses = _context.GetTemporaryExpenses(creationId, user);

            Organizations = _context.GetOfficerOrganizations(user).Select(organization =>
                new SelectListItem
                {
                    Value = organization.Id.ToString(),
                    Text = organization.Name
                });
            
            return Page();
        }

        public IActionResult OnPostAddExpense(int creationId)
        {
            OnGet(creationId);

            var hasError = false;

            if (Purpose.IsNullOrEmpty())
            {
                PurposeError = "Expense name is required";
                hasError = true;
            }

            if (Amount == null)
            {
                AmountError = "Amount is required";
                hasError = true;
            }

            if (hasError)
            {
                return Page();
            }
            
            _context.AddTemporaryExpense(new TemporaryExpense
            {
                Purpose = Purpose!,
                Amount = (int) Amount!,
                Author = User,
                CreationId = creationId
            });

            TemporaryExpenses = _context.GetTemporaryExpenses(creationId, User);

            Purpose = null;
            Amount = null;
            
            ModelState.Clear();

            return Page();
        }
        
        public IActionResult OnPostRemoveExpense(int id, int creationId)
        {
            var temporaryExpense = _context.GetTemporaryExpense(id);
            
            if (temporaryExpense != null)
            {
                _context.RemoveTemporaryExpense(temporaryExpense);
            }

            OnGet(creationId);  

            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPostSubmit(int creationId)
        {
            OnGet(creationId);
            
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

            if (TemporaryExpenses.IsNullOrEmpty())
            {
                GlobalError = "You need to have at least 1 expense";
                hasError = true;
            }

            if (hasError)
            {
                return Page();
            }

            var budgetRequest = new BudgetRequest
            {
                Title = Title!,
                Body = Body!,
                Requester = _context.GetOrganization(Convert.ToInt32(OrganizationId))!,
                DateRequested = DateTime.Now.Date,
                DateNeeded = (DateTime) DateNeeded!
            };

            var expenses = TemporaryExpenses.Select(temporaryExpense => new Expense()
            {
                BudgetRequest = budgetRequest,
                Purpose = temporaryExpense.Purpose,
                Amount = temporaryExpense.Amount
            }).ToList();
            
            _context.AddBudgetRequest(budgetRequest, expenses);

            return RedirectToPage("./Index");
        }

        public IActionResult OnPostCancel()
        {
            return RedirectToPage("./Index");
        }
    }
}