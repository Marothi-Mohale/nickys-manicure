using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NickysManicurePedicure.Data;
using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.Models.Options;
using NickysManicurePedicure.Services;
using NickysManicurePedicure.ViewModels;

namespace NickysManicurePedicure.Controllers;

public class BookingController(
    ApplicationDbContext dbContext,
    IInquiryService inquiryService,
    IOptions<BusinessProfileOptions> businessOptions) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Book Appointment";
        ViewData["Description"] = "Request your manicure or pedicure appointment with Nicky's Manicure & Pedicure in Cape Town.";

        var model = new ServicesPageViewModel
        {
            Business = businessOptions.Value,
            Services = await dbContext.Services
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync(cancellationToken),
            InquiryForm = new InquiryFormViewModel
            {
                InquiryType = InquiryType.Booking,
                SourcePage = "Booking"
            }
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(InquiryFormViewModel model, CancellationToken cancellationToken)
    {
        ViewData["Title"] = model.InquiryType == InquiryType.Booking ? "Book Appointment" : "Contact";

        if (!ModelState.IsValid)
        {
            return await ReturnInvalidModelAsync(model, cancellationToken);
        }

        var (success, message) = await inquiryService.CreateAsync(model, cancellationToken);

        if (success)
        {
            TempData["SuccessMessage"] = message;
            return model.SourcePage switch
            {
                "Services" => RedirectToAction("Index", "Services"),
                "Contact" => RedirectToAction("Index", "Contact"),
                "Booking" => RedirectToAction("Index", "Booking"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        TempData["ErrorMessage"] = message;
        return await ReturnInvalidModelAsync(model, cancellationToken);
    }

    private async Task<IActionResult> ReturnInvalidModelAsync(InquiryFormViewModel model, CancellationToken cancellationToken)
    {
        if (model.InquiryType == InquiryType.General)
        {
            ViewData["Description"] = "Contact Nicky's Manicure & Pedicure for appointments and service enquiries.";
            return View("~/Views/Contact/Index.cshtml", new ContactPageViewModel
            {
                Business = businessOptions.Value,
                InquiryForm = model
            });
        }

        ViewData["Description"] = "Request your manicure or pedicure appointment with Nicky's Manicure & Pedicure in Cape Town.";
        return View("Index", new ServicesPageViewModel
        {
            Business = businessOptions.Value,
            Services = await dbContext.Services
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync(cancellationToken),
            InquiryForm = model
        });
    }
}
