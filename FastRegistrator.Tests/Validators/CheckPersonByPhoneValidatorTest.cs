using FastRegistrator.ApplicationCore.Commands.CheckPersonByPhone;
using FluentValidation.TestHelper;
using System.ComponentModel;

namespace FastRegistrator.Tests.Validators
{
    public class CheckPersonByPhoneValidatorTest
    {
        [Fact]
        [Description("Arrange Command come with regular phone number" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public void Validate_RegularPhoneNumber_PassValidation()
        {
            // Arrange
            var validator = new CheckPersonByPhoneCommandValidator();
            var command = new CheckPersonByPhoneCommand
            {
                PhoneNumber = "89999999999"
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
        }

        [Fact]
        [Description("Arrange Command come with plus at beginning of phone number" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public void Validate_PlusBeforePhoneNumber_PassValidation()
        {
            // Arrange
            var validator = new CheckPersonByPhoneCommandValidator();
            var command = new CheckPersonByPhoneCommand
            {
                PhoneNumber = "+79999999999"
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
        }

        [Fact]
        [Description("Arrange Command come with parentheses in phone number" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public void Validate_ParenthesesInPhoneNumber_PassValidation()
        {
            // Arrange
            var validator = new CheckPersonByPhoneCommandValidator();
            var command = new CheckPersonByPhoneCommand
            {
                PhoneNumber = "8(999)9999999"
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
        }

        [Fact]
        [Description("Arrange Command come with spaces in phone number" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public void Validate_SpacesInPhoneNumber_PassValidation()
        {
            // Arrange
            var validator = new CheckPersonByPhoneCommandValidator();
            var command = new CheckPersonByPhoneCommand
            {
                PhoneNumber = "8 999 999 99 99"
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
        }

        [Fact]
        [Description("Arrange Command come with dashes in phone number" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public void Validate_DashesInPhoneNumber_PassValidation()
        {
            // Arrange
            var validator = new CheckPersonByPhoneCommandValidator();
            var command = new CheckPersonByPhoneCommand
            {
                PhoneNumber = "8-999-999-99-99"
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
        }

        [Fact]
        [Description("Arrange Command come with empty phone number" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public void Validate_EmptyPhoneNumber_FalseValidationAndErrorMessage()
        {
            // Arrange
            var validator = new CheckPersonByPhoneCommandValidator();
            var command = new CheckPersonByPhoneCommand
            {
                PhoneNumber = string.Empty
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                  .WithErrorMessage(CheckPersonByPhoneCommandValidator.MobilePhoneIsEmpty);
        }

        [Fact]
        [Description("Arrange Command come with wrong country code in phone number" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public void Validate_WrongCountryCode_FalseValidationAndErrorMessage()
        {
            // Arrange
            var validator = new CheckPersonByPhoneCommandValidator();
            var command = new CheckPersonByPhoneCommand
            {
                PhoneNumber = "19999999999"
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                  .WithErrorMessage(CheckPersonByPhoneCommandValidator.MobilePhoneHasWrongFormat);
        }

        [Fact]
        [Description("Arrange Command come with wrong count of digits in phone number" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public void Validate_WrongCountOfDigits_FalseValidationAndErrorMessage()
        {
            // Arrange
            var validator = new CheckPersonByPhoneCommandValidator();
            var command = new CheckPersonByPhoneCommand
            {
                PhoneNumber = "899999999991"
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                  .WithErrorMessage(CheckPersonByPhoneCommandValidator.MobilePhoneHasWrongFormat);
        }
    }
}
