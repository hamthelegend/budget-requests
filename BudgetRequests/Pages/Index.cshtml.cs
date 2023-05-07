using BudgetRequests.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BudgetRequests.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    
    private readonly DatabaseContext _context;

    public IndexModel(ILogger<IndexModel> logger, DatabaseContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult OnGet()
    {
        if (!_context.HasSuperAdmin())
        {
            return RedirectToPage("./CreateSuperAdmin/Index");
        }
        return RedirectToPage("./Login/Index");
    }
}