namespace password_validator_nunit.Models;

/// <summary>
/// An enumeration of possible validation errors.
/// </summary>
public enum ValidationErrorEnum
{
    IsEmpty,
    IsTooShort,
    IsTooLong,
    DoesNotContainDigits,
    DoesNotContainCapitalLetters
}
