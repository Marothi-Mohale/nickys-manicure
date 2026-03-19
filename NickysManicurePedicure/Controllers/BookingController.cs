using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NickysManicurePedicure.Data;
using NickysManicurePedicure.Models.Options;
using NickysManicurePedicure.Routing;
using NickysManicurePedicure.Services;
using NickysManicurePedicure.ViewModels;

namespace NickysManicurePedicure.Controllers;

public class BookingController(
    ApplicationDbContext dbContext,
    IBookingRequestService bookingRequestService,
    IOptions<BusinessProfileOptions> businessOptions,
    ILogger<BookingController> logger) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Book Appointment";
        ViewData["Description"] = "Request your manicure or pedicure appointment with Nicky's Manicure & Pedicure in Cape Town.";

        var model = new BookingPageViewModel
        {
            Business = businessOptions.Value,
            Services = await dbContext.Services
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync(cancellationToken),
            BookingForm = new BookingRequestViewModel
            {
                SourcePage = RouteSourcePages.Booking
            }
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(BookingRequestViewModel model, CancellationToken cancellationToken)
    {
        model.SourcePage = RouteSourcePages.Normalize(model.SourcePage, RouteSourcePages.Booking);

        ViewData["Title"] = "Book Appointment";
        ViewData["Description"] = "Request your manicure or pedicure appointment with Nicky's Manicure & Pedicure in Cape Town.";

        if (!ModelState.IsValid)
        {
            logger.LogInformation(
                "Booking form validation failed for source page {SourcePage} and request {TraceIdentifier}.",
                model.SourcePage,
                HttpContext.TraceIdentifier);
            return await ReturnInvalidModelAsync(model, cancellationToken);
        }

        var result = await bookingRequestService.CreateAsync(model, cancellationToken);

        if (result.Success)
        {
            TempData["SuccessMessage"] = result.Message;
            return model.SourcePage switch
            {
                RouteSourcePages.Services => RedirectToAction("Index", "Services"),
                RouteSourcePages.Booking => RedirectToAction("Index", "Booking"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        logger.LogWarning("Booking request submission failed for source page {SourcePage}.", model.SourcePage);
        TempData["ErrorMessage"] = result.Message;
        return await ReturnInvalidModelAsync(model, cancellationToken);
    }

    private async Task<IActionResult> ReturnInvalidModelAsync(BookingRequestViewModel model, CancellationToken cancellationToken)
    {
        ViewData["Description"] = "Request your manicure or pedicure appointment with Nicky's Manicure & Pedicure in Cape Town.";
        var services = await dbContext.Services
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync(cancellationToken);

        return model.SourcePage switch
        {
            RouteSourcePages.Services => View("~/Views/Services/Index.cshtml", new ServicesPageViewModel
            {
                Business = businessOptions.Value,
                Services = services,
                BookingForm = model
            }),
            RouteSourcePages.Home => View("~/Views/Home/Index.cshtml", new HomePageViewModel
            {
                Business = businessOptions.Value,
                FeaturedServices = services.Where(x => x.IsFeatured).ToList(),
                Testimonials = await dbContext.Testimonials
                    .OrderBy(x => x.DisplayOrder)
                    .ToListAsync(cancellationToken),
                FaqItems = await dbContext.FaqItems
                    .OrderBy(x => x.DisplayOrder)
                    .ToListAsync(cancellationToken),
                BookingForm = model
            }),
            _ => View("Index", new BookingPageViewModel
            {
                Business = businessOptions.Value,
                Services = services,
                BookingForm = model
            })
        };
    }
}
