using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.Models.Options;
using NickysManicurePedicure.ViewModels;

namespace NickysManicurePedicure.Controllers;

public class ContactController(IOptions<BusinessProfileOptions> businessOptions) : Controller
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
}
