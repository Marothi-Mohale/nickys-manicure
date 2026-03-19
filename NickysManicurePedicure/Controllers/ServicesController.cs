using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NickysManicurePedicure.Content;
using NickysManicurePedicure.Data;
using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.Models.Options;
using NickysManicurePedicure.Routing;
using NickysManicurePedicure.ViewModels;

namespace NickysManicurePedicure.Controllers;

public class ServicesController(
    ApplicationDbContext dbContext,
    IOptions<BusinessProfileOptions> businessOptions) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Services";
        ViewData["Description"] = "Explore premium manicure, pedicure, gel polish, and occasion-ready nail care services in Cape Town.";

        var model = new ServicesPageViewModel
        {
            Business = businessOptions.Value,
            Services = await dbContext.Services
                .AsNoTracking()
                .Where(x => x.Status == ContentStatus.Published)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync(cancellationToken),
            EditorialImages = SalonImageCatalog.ServiceEditorialImages,
            BookingForm = new BookingRequestViewModel
            {
                SourcePage = RouteSourcePages.Services
            }
        };

        return View(model);
    }
}
