using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NickysManicurePedicure.Models.Options;
using NickysManicurePedicure.ViewModels;

namespace NickysManicurePedicure.Controllers;

public class GalleryController(IOptions<BusinessProfileOptions> businessOptions) : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Gallery";
        ViewData["Description"] = "Preview the elegant finishing style and future nail art gallery of Nicky's Manicure & Pedicure.";

        var model = new GalleryPageViewModel
        {
            Business = businessOptions.Value,
            GalleryItems =
            [
                new() { Title = "Soft Nude Signature", Description = "A timeless nude set designed for clean luxury and everyday confidence." },
                new() { Title = "Bridal Ivory Gloss", Description = "Classic occasion-ready nails with a delicate bridal finish." },
                new() { Title = "Modern French Detail", Description = "Elevated French styling with refined line work and a polished silhouette." },
                new() { Title = "Blush & Gold Accent", Description = "A feminine palette with soft metallic touches suited to celebrations." },
                new() { Title = "Pedicure Perfection", Description = "Fresh, immaculate toe finishes that complement a complete self-care ritual." },
                new() { Title = "Custom Art Concept", Description = "Reserved space for future premium design showcases and bespoke client work." }
            ]
        };

        return View(model);
    }
}
