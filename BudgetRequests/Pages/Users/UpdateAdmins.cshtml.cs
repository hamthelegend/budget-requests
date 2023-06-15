﻿using BudgetRequests.Models;
using BudgetRequests.Models.Admins;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetRequests.Pages.Users;

public class UpdateAdmins : PageModel
{
    private readonly DatabaseContext _context;
    
    public IEnumerable<SelectListItem> Admins { get; set; } = default!;
    
    [BindProperty] public string AssistantDeanId { get; set; }
    [BindProperty] public string DeanId { get; set; }
    [BindProperty] public string StudentAffairsDirectorId { get; set; }

    public UpdateAdmins(DatabaseContext context)
    {
        _context = context;
        Admins = _context.GetAdmins().Select(admin =>
            new SelectListItem
            {
                Value = admin.Id.ToString(),
                Text = $"{admin.FirstName} {admin.LastName}"
            });
    }
    
    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var assistantDean = _context.GetUser(Convert.ToInt32(AssistantDeanId));
        var dean = _context.GetUser(Convert.ToInt32(DeanId));
        var studentAffairsDirector = _context.GetUser(Convert.ToInt32(StudentAffairsDirectorId));

        _context.AddAdminRole(new AdminRole
        {
            Admin = assistantDean!,
            Position = AdminPosition.AssistantDean
        });

        _context.AddAdminRole(new AdminRole
        {
            Admin = dean!,
            Position = AdminPosition.Dean
        });

        _context.AddAdminRole(new AdminRole
        {
            Admin = studentAffairsDirector!,
            Position = AdminPosition.StudentAffairsDirector
        });

        return RedirectToPage("./CollegeAdmins");
    }
}