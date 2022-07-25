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
                  .WithErrorMessage("Mobile phone is empty.");
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
                  .WithErrorMessage("Mobile phone has wrong format.");
        }
    }
}
