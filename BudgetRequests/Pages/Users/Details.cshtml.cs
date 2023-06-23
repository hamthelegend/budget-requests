using BudgetRequests.Data;
using BudgetRequests.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetRequests.Pages.Users;
 
public class DetailsModel : PageModel
{
    private readonly BudgetRequests.Models.DatabaseContext _context;

    public DetailsModel(BudgetRequests.Models.DatabaseContext context)
    {
        _context = context;
    }
    
    public new User User { get; set; }

    public IActionResult OnGet(int? id)
    {
        var user = _context.GetUser(id ?? -1);

        if (user == null)
        {
            return NotFound();
        }

        User = user;

        return Page();
    }
}