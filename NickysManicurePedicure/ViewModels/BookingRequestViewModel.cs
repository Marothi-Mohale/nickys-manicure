using System.ComponentModel.DataAnnotations;
using NickysManicurePedicure.Routing;

namespace NickysManicurePedicure.ViewModels;

public class BookingRequestViewModel : IValidatableObject
{
    [Display(Name = "Full name")]
    [Required(ErrorMessage = "Please enter your full name so we know who to contact.")]
    [StringLength(120, ErrorMessage = "Your full name must be 120 characters or fewer.")]
    public string FullName { get; set; } = string.Empty;

    [Display(Name = "Phone number")]
    [Required(ErrorMessage = "Please enter a phone number we can use to confirm your appointment.")]
    [Phone(ErrorMessage = "Please enter a valid phone number, including your area or country code if needed.")]
    [StringLength(30, ErrorMessage = "Your phone number must be 30 characters or fewer.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Display(Name = "Email address")]
    [Required(ErrorMessage = "Please enter your email address so we can send booking updates if needed.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [StringLength(200, ErrorMessage = "Your email address must be 200 characters or fewer.")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Preferred service")]
    [Required(ErrorMessage = "Please tell us which service you would like to book.")]
    [StringLength(120, ErrorMessage = "Please keep your preferred service under 120 characters.")]
    public string PreferredService { get; set; } = string.Empty;

    [Display(Name = "Preferred date")]
    [Required(ErrorMessage = "Please choose your preferred appointment date.")]
    [DataType(DataType.Date)]
    public DateOnly? PreferredDate { get; set; }

    [Display(Name = "Preferred time")]
    [Required(ErrorMessage = "Please choose your preferred appointment time.")]
    [DataType(DataType.Time)]
    public TimeOnly? PreferredTime { get; set; }

    [Display(Name = "Message")]
    [Required(ErrorMessage = "Please share a few details about your appointment request.")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "Please enter between 10 and 2000 characters.")]
    public string Message { get; set; } = string.Empty;

    [StringLength(30, ErrorMessage = "The source page value is too long.")]
    public string SourcePage { get; set; } = "Booking";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);

        if (PreferredDate is not null && PreferredDate < today)
        {
            yield return new ValidationResult(
                "Please choose a preferred date that is today or later.",
                [nameof(PreferredDate)]);
        }

        if (!string.IsNullOrWhiteSpace(Message) && Message.Trim().Length < 10)
        {
            yield return new ValidationResult(
                "Please add a little more detail so we can prepare for your appointment.",
                [nameof(Message)]);
        }

        if (!RouteSourcePages.IsKnown(SourcePage))
        {
            yield return new ValidationResult(
                "The booking source was not recognized.",
                [nameof(SourcePage)]);
        }
    }
}
