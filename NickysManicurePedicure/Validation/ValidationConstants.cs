namespace NickysManicurePedicure.Validation;

public static class ValidationConstants
{
    public const string PhoneSanityPattern = @"^\+?[0-9 ()\-]{7,30}$";
    public const string SlugPattern = @"^[a-z0-9]+(?:-[a-z0-9]+)*$";
}
