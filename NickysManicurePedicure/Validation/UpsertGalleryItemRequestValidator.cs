using FluentValidation;
using NickysManicurePedicure.Dtos.Requests.Admin;

namespace NickysManicurePedicure.Validation;

public sealed class UpsertGalleryItemRequestValidator : AbstractValidator<UpsertGalleryItemRequest>
{
    public UpsertGalleryItemRequestValidator()
    {
        RuleFor(x => x.Title)
            .TrimmedRequiredText(160, "Title");

        RuleFor(x => x.Description)
            .MaximumLength(800).WithMessage("Description must be 800 characters or fewer.");

        RuleFor(x => x.Category)
            .MaximumLength(60).WithMessage("Category must be 60 characters or fewer.");

        RuleFor(x => x.ImageUrl)
            .TrimmedRequiredText(500, "Image URL")
            .Must(BeValidUrlOrAbsolutePath)
            .WithMessage("Image URL must be a valid absolute URL or site-relative path.");

        RuleFor(x => x.ThumbnailUrl)
            .MaximumLength(500).WithMessage("Thumbnail URL must be 500 characters or fewer.")
            .Must(url => string.IsNullOrWhiteSpace(url) || BeValidUrlOrAbsolutePath(url))
            .WithMessage("Thumbnail URL must be a valid absolute URL or site-relative path.");

        RuleFor(x => x.AltText)
            .TrimmedRequiredText(220, "Alt text");

        RuleFor(x => x.Status)
            .Must(status => status is "Draft" or "Published" or "Archived")
            .WithMessage("Status must be Draft, Published, or Archived.");

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0).WithMessage("Display order must be 0 or greater.");
    }

    private static bool BeValidUrlOrAbsolutePath(string url)
    {
        if (Uri.TryCreate(url, UriKind.Absolute, out var absoluteUri))
        {
            return absoluteUri.Scheme == Uri.UriSchemeHttp || absoluteUri.Scheme == Uri.UriSchemeHttps;
        }

        return url.StartsWith('/');
    }
}
