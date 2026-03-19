using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NickysManicurePedicure.Data;
using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.Models.Options;
using NickysManicurePedicure.ViewModels;

namespace NickysManicurePedicure.Controllers;

public class AboutController(
    ApplicationDbContext dbContext,
    IOptions<BusinessProfileOptions> businessOptions) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewData["Title"] = "About";
        ViewData["Description"] = "Meet Nicky and learn about the thoughtful, high-end nail care experience trusted by clients across Cape Town.";

        var model = new AboutPageViewModel
        {
            Business = businessOptions.Value,
            Testimonials = await dbContext.Testimonials
                .AsNoTracking()
                .Where(x => x.Status == ContentStatus.Published && x.IsApproved)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync(cancellationToken)
        };

        return View(model);
    }
}
