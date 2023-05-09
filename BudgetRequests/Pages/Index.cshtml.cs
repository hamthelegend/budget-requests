using BudgetRequests.Data;
using BudgetRequests.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

    public async Task<IActionResult> OnGetAsync()
    {
        if (!_context.HasSuperAdmin())
        {
            return RedirectToPage("./CreateSuperAdmin/Index");
        }

        var userId = HttpContext.Session.GetInt32(Session.UserIdKey);
        if (userId == null) return RedirectToPage("./Login/Index");
        var user = _context.GetUser((int)userId);
        if (user == null) return RedirectToPage("./Login/Index");
        return RedirectToPage("./HomePage/Index");
    }
}