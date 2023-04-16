using password_validator_nunit.Models;

namespace password_validator_nunit.Utils;

public interface IPasswordValidator
{
    void Initialize(int minLength, int maxLength, bool mustContainDigits, bool mustContainCapitalLetters);
    ValidationResult Validate(string password);
}

