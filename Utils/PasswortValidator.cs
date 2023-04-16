namespace password_validator_nunit.Utils;

using System;
using System.Collections.Generic;
using password_validator_nunit.Models;

public class PasswordValidator : IPasswordValidator
{
    private int minLength;
    private int maxLength;
    private bool mustContainDigits;
    private bool mustContainCapitalLetters;

    /// <summary>
    /// Initializes the password validator with the given criteria.
    /// </summary>
    /// <param name="minLength">The minimum length of the password.</param>
    /// <param name="maxLength">The maximum length of the password.</param>
    /// <param name="mustContainDigits">Whether the password must contain digits.</param>
    /// <param name="mustContainCapitalLetters">Whether the password must contain capital letters.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown when the minimum password length is zero or negative, 
    /// or the maximum password length is greater than 255.</exception>
    /// <exception cref="ArgumentException">Thrown when the minimum password length is greater than the maximum 
    /// password length.</exception>
    public void Initialize(int minLength, int maxLength, bool mustContainDigits, bool mustContainCapitalLetters)
    {
        if (minLength <= 0)
        {
            throw new IndexOutOfRangeException("Minimum password length cannot be zero or negative.");
        }

        if (maxLength > 255)
        {
            throw new IndexOutOfRangeException("Maximum password length cannot be greater than 255.");
        }

        if (minLength > maxLength)
        {
            throw new ArgumentException("Minimum password length cannot be greater than maximum password length.");
        }

        this.minLength = minLength;
        this.maxLength = maxLength;
        this.mustContainDigits = mustContainDigits;
        this.mustContainCapitalLetters = mustContainCapitalLetters;
    }

    /// <summary>
    /// Validates the given password based on the criteria specified during initialization.
    /// </summary>
    /// <param name="password">The password to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> object that indicates whether the password is valid or not, 
    /// and if not, what errors were encountered.</returns>
    public ValidationResult Validate(string password)
    {
        var errors = new List<ValidationErrorEnum>();
        var validationResult = new ValidationResult();

        if (password == null)
        {
            validationResult.IsCorrect = false;
            validationResult.Errors = new[] { ValidationErrorEnum.IsEmpty };
            return validationResult;
        }

        if (password.Length == 0)
        {
            errors.Add(ValidationErrorEnum.IsEmpty);
        }

        if (password.Length < minLength)
        {
            errors.Add(ValidationErrorEnum.IsTooShort);
        }

        if (password.Length > maxLength)
        {
            errors.Add(ValidationErrorEnum.IsTooLong);
        }

        if (mustContainDigits && !HasDigits(password))
        {
            errors.Add(ValidationErrorEnum.DoesNotContainDigits);
        }

        if (mustContainCapitalLetters && !HasCapitalLetters(password))
        {
            errors.Add(ValidationErrorEnum.DoesNotContainCapitalLetters);
        }

        validationResult = new ValidationResult()
        {
            IsCorrect = errors.Count == 0,
            Errors = errors.Count == 0 ? Array.Empty<ValidationErrorEnum>() : errors.Distinct().ToArray()
        };
        return validationResult;
    }

    private static bool HasDigits(string password)
    {
        foreach (char c in password)
        {
            if (Char.IsDigit(c))
            {
                return true;
            }
        }

        return false;
    }

    private static bool HasCapitalLetters(string password)
    {
        foreach (char c in password)
        {
            if (Char.IsUpper(c))
            {
                return true;
            }
        }

        return false;
    }
}
