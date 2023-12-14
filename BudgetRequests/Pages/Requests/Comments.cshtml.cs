using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetRequests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BudgetRequests.Models;
using BudgetRequests.Models.BudgetRequests;
using BudgetRequests.Models.Comments;

namespace BudgetRequests.Pages.Requests;

public class CommentsModel : PageModel
{
    private readonly DatabaseContext _context;

    public CommentsModel(DatabaseContext context)
    {
        _context = context;
    }

    public BudgetRequest BudgetRequest { get; set; } = default!;

    public List<Comment> Comments { get; set; } = new();

    [BindProperty] public string? Comment { get; set; } = default!;
    public IActionResult OnGet(int? id)
    {
        var user = HttpContext.Session.GetLoggedInUser(_context);
        var budgetRequest = _context.GetBudgetRequest(id ?? -1);

        if (user == null || budgetRequest == null)
        {
            return NotFound();
        }
            
        BudgetRequest = budgetRequest;
        Comments = _context.GetComments(budgetRequest);

        return Page();
    }

    public IActionResult OnPostAdd(int? id)
    {
        OnGet(id);

        var user = HttpContext.Session.GetLoggedInUser(_context);

        if (Comment != null)
        {
            var comment = new Comment
            {
                BudgetRequest = BudgetRequest,
                Commenter = user,
                Body = Comment
            };
            _context.AddComment(comment);
        }
            
        _context.SaveChanges();

        Comment = "";
        ModelState.Clear();

        OnGet(id);
        return Page();
    }

    public IActionResult OnPostCancel(int? id)
    {
        return RedirectToPage("./Details", new { id });
    }

}