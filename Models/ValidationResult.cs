namespace password_validator_nunit.Models;
/// <summary>
/// The result of validating a password.
/// </summary>
public class ValidationResult
{
    /// <summary>
    /// Returns true only if a password matches all the rules.
    /// </summary>
    public bool IsCorrect { get; set; }

    /// <summary>
    /// If IsCorrect is true then this property returns an empty array. Otherwise it returns all found errors.
    /// Errors cannot be duplicated. This property must not be null.
    /// </summary>
    public ValidationErrorEnum[] Errors { get; set; }
}
