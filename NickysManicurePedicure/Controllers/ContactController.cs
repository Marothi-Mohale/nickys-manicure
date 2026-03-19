using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.Models.Options;
using NickysManicurePedicure.Routing;
using NickysManicurePedicure.Services;
using NickysManicurePedicure.ViewModels;

namespace NickysManicurePedicure.Controllers;

public class ContactController(
    IOptions<BusinessProfileOptions> businessOptions,
    IInquiryService inquiryService,
    ILogger<ContactController> logger) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        ViewData["Title"] = "Contact";
        ViewData["Description"] = "Get in touch with Nicky's Manicure & Pedicure for enquiries, appointments, and premium nail care in Mowbray, Cape Town.";

        var model = new ContactPageViewModel
        {
            Business = businessOptions.Value,
            InquiryForm = new InquiryFormViewModel
            {
                InquiryType = InquiryType.General,
                SourcePage = RouteSourcePages.Contact,
                Message = "Hello Nicky, I would love to know more about your services."
            }
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(InquiryFormViewModel model, CancellationToken cancellationToken)
    {
        model.SourcePage = RouteSourcePages.Normalize(model.SourcePage, RouteSourcePages.Contact);

        ViewData["Title"] = "Contact";
        ViewData["Description"] = "Get in touch with Nicky's Manicure & Pedicure for enquiries, appointments, and premium nail care in Mowbray, Cape Town.";

        if (!ModelState.IsValid)
        {
            logger.LogInformation(
                "Contact form validation failed for request {TraceIdentifier}.",
                HttpContext.TraceIdentifier);

            return View("Index", new ContactPageViewModel
            {
                Business = businessOptions.Value,
                InquiryForm = model
            });
        }

        var result = await inquiryService.CreateAsync(model, cancellationToken);

        if (!result.Success)
        {
            logger.LogWarning(
                "Contact form submission failed for request {TraceIdentifier}.",
                HttpContext.TraceIdentifier);
            TempData["ErrorMessage"] = result.Message;
            return View("Index", new ContactPageViewModel
            {
                Business = businessOptions.Value,
                InquiryForm = model
            });
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction(nameof(Index));
    }
}
