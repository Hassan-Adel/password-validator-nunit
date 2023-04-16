using NUnit.Framework;
using password_validator_nunit.Models;
using password_validator_nunit.Utils;

namespace password_validator_nunit.Tests.Utils;

[TestFixture]
public class PasswordValidatorTests
{
    private PasswordValidator validator;

    [SetUp]
    public void SetUp()
    {
        validator = new PasswordValidator();
    }

    [Test]
    public void Initialize_MinLengthZero_ThrowsIndexOutOfRangeException()
    {
        Assert.Throws<IndexOutOfRangeException>(() => validator.Initialize(0, 20, true, true));
    }

    [Test]
    public void Initialize_MaxLengthGreaterThan255_ThrowsIndexOutOfRangeException()
    {
        Assert.Throws<IndexOutOfRangeException>(() => validator.Initialize(8, 256, true, true));
    }

    [Test]
    public void Initialize_MinLengthGreaterThanMaxLength_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => validator.Initialize(20, 8, true, true));
    }

    [Test]
    public void Validate_WhenPasswordIsNull_ReturnsIsEmptyError()
    {
        validator.Initialize(8, 20, true, true);

        var result = validator.Validate(null);

        Assert.That(result.Errors.Contains(ValidationErrorEnum.IsEmpty));
        Assert.IsFalse(result.IsCorrect);
    }

    [Test]
    public void Validate_WhenPasswordIsEmpty_ReturnsIsEmptyError()
    {
        validator.Initialize(8, 20, true, true);

        var result = validator.Validate("");

        Assert.That(result.Errors.Contains(ValidationErrorEnum.IsEmpty));
        Assert.IsFalse(result.IsCorrect);
    }

    [Test]
    public void Validate_WhenPasswordIsTooShort_ReturnsIsTooShortError()
    {
        validator.Initialize(8, 20, true, true);

        var result = validator.Validate("abcde");

        Assert.That(result.Errors.Contains(ValidationErrorEnum.IsTooShort));
        Assert.IsFalse(result.IsCorrect);
    }

    [Test]
    public void Validate_WhenPasswordIsTooLong_ReturnsIsTooLongError()
    {
        validator.Initialize(8, 20, true, true);

        var result = validator.Validate("abcdefghijkmnopqrstuvwxyz1234567890");

        Assert.That(result.Errors.Contains(ValidationErrorEnum.IsTooLong));
        Assert.IsFalse(result.IsCorrect);
    }

    [Test]
    public void Validate_WhenPasswordDoesNotContainDigitsAndCapitalLetters_ReturnsDoesNotContainDigitsAndCapitalLettersErrors()
    {
        validator.Initialize(8, 20, true, true);

        var result = validator.Validate("abcdefg");

        Assert.That(result.Errors.Contains(ValidationErrorEnum.DoesNotContainDigits));
        Assert.That(result.Errors.Contains(ValidationErrorEnum.DoesNotContainCapitalLetters));
        Assert.IsFalse(result.IsCorrect);
    }

    [Test]
    public void Validate_WhenPasswordDoesNotContainDigits_ReturnsDoesNotContainDigitsError()
    {
        validator.Initialize(8, 20, true, true);

        var result = validator.Validate("Abcdefgh");

        Assert.That(result.Errors.Contains(ValidationErrorEnum.DoesNotContainDigits));
        Assert.IsFalse(result.IsCorrect);
    }

    [Test]
    public void Validate_WhenPasswordDoesNotContainCapitalLetters_ReturnsDoesNotContainCapitalLettersError()
    {
        validator.Initialize(8, 20, true, true);

        var result = validator.Validate("abcdefg1");

        Assert.That(result.Errors.Contains(ValidationErrorEnum.DoesNotContainCapitalLetters));
        Assert.IsFalse(result.IsCorrect);
    }

    [Test]
    public void Validate_WhenPasswordIsValid_ReturnsIsCorrect()
    {
        validator.Initialize(8, 20, true, true);

        var result = validator.Validate("Abcdefg1");

        Assert.IsTrue(result.IsCorrect);
        Assert.IsEmpty(result.Errors);
    }

    [Test]
    public void Validate_WhenPasswordContainsRepeatingErrors_ReturnsDistinctErrorsOnly()
    {
        validator.Initialize(8, 20, true, true);

        var result = validator.Validate("acdfg");

        Assert.That(result.Errors.Contains(ValidationErrorEnum.DoesNotContainCapitalLetters));
        Assert.That(result.Errors.Contains(ValidationErrorEnum.DoesNotContainDigits));
        Assert.That(result.Errors.Length, Is.EqualTo(result.Errors.Distinct().Count()));
    }
}
