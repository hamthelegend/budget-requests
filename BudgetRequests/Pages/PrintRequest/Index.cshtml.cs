using BudgetRequests.Data;
using BudgetRequests.Helpers;
using BudgetRequests.Models;
using BudgetRequests.Models.BudgetRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SelectPdf;

namespace BudgetRequests.Pages.PrintRequest; 

public class IndexModel : PageModel {
    private readonly DatabaseContext _context;
    private readonly IServiceProvider _serviceProvider;
    private readonly IViewRenderService _viewRenderService;

    public IndexModel(DatabaseContext context, IServiceProvider serviceProvider, IViewRenderService viewRenderService) {
        _context = context;
        _serviceProvider = serviceProvider;
        _viewRenderService = viewRenderService;
    }
    
    public new User User { get; set; } = default!;
    public BudgetRequest BudgetRequest { get; set; } = default!;
    public List<Expense> Expenses { get; set; } = default!;
    public Signatories Signatories { get; set; } = default!;
    public string Status { get; set; }

    public IActionResult OnGet(int? id) {
        var user = HttpContext.Session.GetLoggedInUser(_context);
        var budgetRequest = _context.GetBudgetRequest(id ?? -1);
        
        if (user == null || budgetRequest == null)
        {
            return NotFound();
        }

        User = user;
        BudgetRequest = budgetRequest;
        Expenses = _context.GetExpenses(budgetRequest);
        Signatories = _context.GetSignatories(budgetRequest);

        return Page();
    }

    public IActionResult OnPostDownloadFile(int? requestId) {
        var budgetRequest = _context.GetBudgetRequest(requestId ?? -1);
        var html = _viewRenderService.RenderToString("Pages/PrintRequest/_pdfView.cshtml", BudgetRequest.Id).Result;


        PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
            "A4", true);

        PdfPageOrientation pdfOrientation =
            (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                "Portrait", true);

        int webPageWidth = 1024;
        int webPageHeight = 1024;

        // instantiate a html to pdf converter object
        HtmlToPdf converter = new HtmlToPdf();

        // set converter options
        converter.Options.PdfPageSize = pageSize;
        converter.Options.PdfPageOrientation = pdfOrientation;
        converter.Options.WebPageWidth = webPageWidth;
        converter.Options.WebPageHeight = webPageHeight;

        // create a new pdf document converting an url
        PdfDocument doc = converter.ConvertHtmlString(html, "https://localhost:43368/");

        // save pdf document
        byte[] pdf = doc.Save();

        // close pdf document
        doc.Close();

        FileResult fileResult = new FileContentResult(pdf, "application/pdf");
        fileResult.FileDownloadName = "Document.pdf";

        return fileResult;
    }
}