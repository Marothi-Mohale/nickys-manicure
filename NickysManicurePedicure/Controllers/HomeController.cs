using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NickysManicurePedicure.Data;
using NickysManicurePedicure.Models;
using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.Models.Options;
using NickysManicurePedicure.Routing;
using NickysManicurePedicure.ViewModels;

namespace NickysManicurePedicure.Controllers;

public class HomeController(
    ApplicationDbContext dbContext,
    IOptions<BusinessProfileOptions> businessOptions,
    ILogger<HomeController> logger) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Luxury Manicure & Pedicure in Cape Town";
        ViewData["Description"] = "Discover refined manicure and pedicure services in Mowbray, Cape Town. Trusted care, elegant finishes, and premium appointment experiences.";

        var model = new HomePageViewModel
        {
            Business = businessOptions.Value,
            FeaturedServices = await dbContext.Services
                .AsNoTracking()
                .Where(x => x.Status == ContentStatus.Published)
                .Where(x => x.IsFeatured)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync(cancellationToken),
            Testimonials = await dbContext.Testimonials
                .AsNoTracking()
                .Where(x => x.Status == ContentStatus.Published && x.IsApproved)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync(cancellationToken),
            FaqItems = await dbContext.FaqItems
                .AsNoTracking()
                .Where(x => x.Status == ContentStatus.Published && x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync(cancellationToken),
            BookingForm = new BookingRequestViewModel
            {
                SourcePage = RouteSourcePages.Home
            }
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        ViewData["Title"] = "Privacy";
        ViewData["Description"] = "Read how Nicky's Manicure & Pedicure handles your personal information and booking enquiries.";
        return View();
    }

    public IActionResult Terms()
    {
        ViewData["Title"] = "Terms";
        ViewData["Description"] = "Terms and booking information for Nicky's Manicure & Pedicure.";
        return View();
    }

    [HttpGet("/status/{code:int}")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult StatusCodePage(int code)
    {
        Response.StatusCode = code;
        return View("StatusCode", code);
    }

    [HttpGet("/error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        logger.LogError(
            "Rendering error page for request {Path} with trace identifier {TraceIdentifier}.",
            HttpContext.Request.Path,
            HttpContext.TraceIdentifier);

        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
