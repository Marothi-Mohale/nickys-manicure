using NickysManicurePedicure.ViewModels;

namespace NickysManicurePedicure.Content;

public static class SalonImageCatalog
{
    public static SalonImageViewModel HomeHero => new()
    {
        ImageUrl = "/images/salon/home/nicky-hero-manicure.jpg",
        AltText = "Bright red manicure with glossy square nails and delicate crystal accents.",
        Caption = "Signature manicure styling"
    };

    public static IReadOnlyList<SalonImageViewModel> HomePreviewImages =>
    [
        new()
        {
            ImageUrl = "/images/salon/home/nicky-home-manicure-classic.jpg",
            AltText = "Pink sculpted manicure with floral accents and a glossy luxury finish.",
            Caption = "Statement manicure"
        },
        new()
        {
            ImageUrl = "/images/salon/home/nicky-home-pedicure-luxury.jpg",
            AltText = "Soft blush pedicure with glossy polish and clean premium presentation.",
            Caption = "Soft blush pedicure"
        },
        new()
        {
            ImageUrl = "/images/salon/home/nicky-home-nail-detail.jpg",
            AltText = "Close-up of neon pink French-tip nails with glossy detail and salon lighting.",
            Caption = "Detail-focused finish"
        }
    ];

    public static IReadOnlyList<SalonImageViewModel> ServiceEditorialImages =>
    [
        new()
        {
            ImageUrl = "/images/salon/services/nicky-service-manicure-gel.jpg",
            AltText = "Graphic black-and-white manicure design on long square nails.",
            Caption = "Creative gel manicure"
        },
        new()
        {
            ImageUrl = "/images/salon/home/nicky-home-pedicure-luxury.jpg",
            AltText = "Glossy pale pink pedicure styled for a clean premium salon presentation.",
            Caption = "Polished pedicure care"
        }
    ];

    public static SalonImageViewModel AboutSignature => new()
    {
        ImageUrl = "/images/salon/home/nicky-home-manicure-classic.jpg",
        AltText = "Detailed pink luxury manicure with floral art and a polished salon finish.",
        Caption = "Personal, detailed artistry"
    };
}
