using BudgetRequests.Models;
using BudgetRequests.Models.Admins;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetRequests.Pages.Users;

public class CollegeAdmins : PageModel
{
    private readonly BudgetRequests.Models.DatabaseContext _context;

    public User? AssistantDean { get; set; } = default!;
    public User? Dean { get; set; } = default!;
    public User? StudentAffairsDirector { get; set; } = default!;

    public CollegeAdmins(BudgetRequests.Models.DatabaseContext context)
    {
        _context = context;
    }
    
    public IActionResult OnGetAsync()
    {
        AssistantDean = _context.GetAdmin(AdminPosition.AssistantDean);
        Dean = _context.GetAdmin(AdminPosition.Dean);
        StudentAffairsDirector = _context.GetAdmin(AdminPosition.StudentAffairsDirector);
        return Page();
    }
}