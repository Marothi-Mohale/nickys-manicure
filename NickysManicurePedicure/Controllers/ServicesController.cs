using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NickysManicurePedicure.Data;
using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.Models.Options;
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
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync(cancellationToken),
            InquiryForm = new InquiryFormViewModel
            {
                InquiryType = InquiryType.Booking,
                SourcePage = "Services"
            }
        };

        return View(model);
    }
}
