using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.Models.Options;
using NickysManicurePedicure.Services;
using NickysManicurePedicure.ViewModels;

namespace NickysManicurePedicure.Controllers;

public class ContactController(
    IOptions<BusinessProfileOptions> businessOptions,
    IInquiryService inquiryService) : Controller
{
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
                SourcePage = "Contact",
                Message = "Hello Nicky, I would love to know more about your services."
            }
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(InquiryFormViewModel model, CancellationToken cancellationToken)
    {
        ViewData["Title"] = "Contact";
        ViewData["Description"] = "Get in touch with Nicky's Manicure & Pedicure for enquiries, appointments, and premium nail care in Mowbray, Cape Town.";

        if (!ModelState.IsValid)
        {
            return View("Index", new ContactPageViewModel
            {
                Business = businessOptions.Value,
                InquiryForm = model
            });
        }

        var result = await inquiryService.CreateAsync(model, cancellationToken);

        if (!result.Success)
        {
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
