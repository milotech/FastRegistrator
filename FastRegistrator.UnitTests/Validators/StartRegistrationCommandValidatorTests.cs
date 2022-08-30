using FastRegistrator.Application.Commands.StartRegistration;
using FluentValidation.TestHelper;
using System.ComponentModel;
using static FastRegistrator.UnitTests.Constants;

namespace FastRegistrator.UnitTests.Validators
{
    public class StartRegistrationCommandValidatorTests
        : TestWithDbContext
    {
        [Fact]
        [Description("Arrange Command come with regular phone number" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public async Task Validate_RegularPhoneNumber_PassValidation()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                PhoneNumber = "89999999999"
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
        }

        [Fact]
        [Description("Arrange Command come with plus at beginning of phone number" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public async Task Validate_PlusBeforePhoneNumber_PassValidation()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                PhoneNumber = "+79999999999"
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
        }

        [Fact]
        [Description("Arrange Command come with parentheses in phone number" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public async Task Validate_ParenthesesInPhoneNumber_PassValidation()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                PhoneNumber = "8(999)9999999"
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
        }

        [Fact]
        [Description("Arrange Command come with spaces in phone number" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public async Task Validate_SpacesInPhoneNumber_PassValidation()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                PhoneNumber = "8 999 999 99 99"
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
        }

        [Fact]
        [Description("Arrange Command come with dashes in phone number" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public async Task Validate_DashesInPhoneNumber_PassValidation()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                PhoneNumber = "8-999-999-99-99"
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
        }

        [Fact]
        [Description("Arrange Command come with empty phone number" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public async Task Validate_EmptyPhoneNumber_FalseValidationAndErrorMessage()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                PhoneNumber = string.Empty
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                  .WithErrorMessage(StartRegistrationCommandValidator.PhoneNumberIsEmpty);
        }



        [Fact]
        [Description("Arrange Command come with wrong count of digits in phone number" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public async Task Validate_TooLongPhoneNumber_FalseValidationAndErrorMessage()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                PhoneNumber = "89999999999111111"
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                  .WithErrorMessage(StartRegistrationCommandValidator.PhoneNumberTooLong);
        }

        [Fact]
        [Description("Arrange Command come with regular first name" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public async Task Validate_RegularFirstName_PassValidation()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                FirstName = FIRST_NAME
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        [Description("Arrange Command come with empty first name" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public async Task Validate_EmptyFirstName_FalseValidationAndErrorMessage()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                FirstName = string.Empty
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                  .WithErrorMessage(StartRegistrationCommandValidator.FirstNameIsEmpty);
        }

        [Fact]
        [Description("Arrange Command come with regular last name" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public async Task Validate_RegularLastName_PassValidation()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                LastName = LAST_NAME
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        [Description("Arrange Command come with empty last name" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public async Task Validate_EmptyLastName_FalseValidationAndErrorMessage()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                LastName = string.Empty
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                  .WithErrorMessage(StartRegistrationCommandValidator.LastNameIsEmpty);
        }

        [Fact]
        [Description("Arrange Command come with correct PassportNumber" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public async Task Validate_CorrectPassportNumber_PassValidation()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                PassportNumber = PASSPORT_NUMBER
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.PassportNumber);
        }

        [Fact]
        [Description("Arrange Command come with wrong count of digits in PassportNumber" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public async Task Validate_IncorrectPassportNumber_FalseValidationAndErrorMessage()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                PassportNumber = "1111 1111111",
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PassportNumber)
                  .WithErrorMessage(StartRegistrationCommandValidator.PassportNumberHasWrongFormat);
        }

        [Fact]
        [Description("Arrange Command come with empty PassportNumber" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public async Task Validate_EmptyPassportNumber_FalseValidationAndErrorMessage()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                PassportNumber = string.Empty,
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PassportNumber)
                  .WithErrorMessage(StartRegistrationCommandValidator.PassportNumberIsEmpty);
        }

        [Fact]
        [Description("Arrange Command come with correct INN" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public async Task Validate_CorrectINN_PassValidation()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                Inn = INN
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Inn);
        }

        [Fact]
        [Description("Arrange Command come with whitespace symbols in INN" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public async Task Validate_WhitespacesINN_PassValidation()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                Inn = "111 111 111 111"
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Inn);
        }

        [Fact]
        [Description("Arrange Command come with wrong count of digits in INN" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public async Task Validate_WrongCountDigitsInn_FalseValidationAndErrorMessage()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                Inn = "111 111 111 111 1"
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Inn)
                  .WithErrorMessage(StartRegistrationCommandValidator.InnHasWrongFormat);
        }

        [Fact]
        [Description("Arrange Command come with empty INN" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public async Task Validate_EmptyInn_PassValidation()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                Inn = string.Empty
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Inn);
        }

        [Fact]
        [Description("Arrange Command come with regular form data" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public async Task Validate_RegularFromData_PassValidation()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                FormData = FORM_DATA
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.FormData);
        }

        [Fact]
        [Description("Arrange Command come with empty form data" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public async Task Validate_EmptyFormData_FalseValidationAndErrorMessage()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                FormData = string.Empty
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FormData)
                  .WithErrorMessage(StartRegistrationCommandValidator.FormDataIsEmpty);
        }
    }
}
