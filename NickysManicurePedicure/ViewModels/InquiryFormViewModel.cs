using System.ComponentModel.DataAnnotations;
using NickysManicurePedicure.Models.Entities;

namespace NickysManicurePedicure.ViewModels;

public class InquiryFormViewModel
{
    public InquiryType InquiryType { get; set; } = InquiryType.Booking;

    [Display(Name = "Full name")]
    [Required(ErrorMessage = "Please enter your full name.")]
    [StringLength(120)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your email address.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Phone number")]
    [Required(ErrorMessage = "Please enter your phone number.")]
    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    [StringLength(30)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Display(Name = "Preferred service")]
    [StringLength(120)]
    public string? ServiceInterest { get; set; }

    [Display(Name = "Preferred appointment date")]
    [DataType(DataType.Date)]
    public DateOnly? PreferredDate { get; set; }

    [Required(ErrorMessage = "Please tell us how we can help.")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "Please enter at least 10 characters.")]
    public string Message { get; set; } = string.Empty;

    public string SourcePage { get; set; } = "Home";

    public string SubmitLabel => InquiryType == InquiryType.Booking ? "Request Appointment" : "Send Message";
}
