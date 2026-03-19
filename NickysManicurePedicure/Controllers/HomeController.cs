using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NickysManicurePedicure.Data;
using NickysManicurePedicure.Models;
using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.Models.Options;
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
                .Where(x => x.IsFeatured)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync(cancellationToken),
            Testimonials = await dbContext.Testimonials
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync(cancellationToken),
            FaqItems = await dbContext.FaqItems
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync(cancellationToken),
            BookingForm = new BookingRequestViewModel
            {
                SourcePage = "Home"
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

    [ActionName("StatusCode")]
    [Route("Home/StatusCode")]
    public IActionResult StatusCodePage(int code)
    {
        Response.StatusCode = code;
        return View(code);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        logger.LogWarning("Rendering error page for request {Path}.", HttpContext.Request.Path);

        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
