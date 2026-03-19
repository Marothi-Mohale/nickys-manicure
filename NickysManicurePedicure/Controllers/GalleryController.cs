using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NickysManicurePedicure.Data;
using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.Models.Options;
using NickysManicurePedicure.ViewModels;

namespace NickysManicurePedicure.Controllers;

public class GalleryController(
    ApplicationDbContext dbContext,
    IOptions<BusinessProfileOptions> businessOptions) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Gallery";
        ViewData["Description"] = "Preview the elegant finishing style and future nail art gallery of Nicky's Manicure & Pedicure.";

        var model = new GalleryPageViewModel
        {
            Business = businessOptions.Value,
            GalleryItems = await dbContext.GalleryItems
                .AsNoTracking()
                .Where(x => x.Status == ContentStatus.Published)
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new GalleryItemViewModel
                {
                    Title = x.Title,
                    Description = x.Description ?? string.Empty
                })
                .ToListAsync(cancellationToken)
        };

        return View(model);
    }
}
