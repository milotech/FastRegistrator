using FastRegistrator.ApplicationCore.Commands.StartRegistration;
using FluentValidation.TestHelper;
using System.ComponentModel;
using static FastRegistrator.Tests.Constants;

namespace FastRegistrator.Tests.Validators
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
                  .WithErrorMessage(StartRegistrationCommandValidator.MobilePhoneIsEmpty);
        }

        [Fact]
        [Description("Arrange Command come with wrong country code in phone number" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public async Task Validate_WrongCountryCode_FalseValidationAndErrorMessage()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                PhoneNumber = "19999999999"
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                  .WithErrorMessage(StartRegistrationCommandValidator.MobilePhoneHasWrongFormat);
        }

        [Fact]
        [Description("Arrange Command come with wrong count of digits in phone number" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public async Task Validate_WrongCountOfDigits_FalseValidationAndErrorMessage()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                PhoneNumber = "899999999991"
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                  .WithErrorMessage(StartRegistrationCommandValidator.MobilePhoneHasWrongFormat);
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
        [Description("Arrange Command come with regular series" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public async Task Validate_RegularSeries_PassValidation()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                Series = SERIES
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Series);
        }

        [Fact]
        [Description("Arrange Command come with redundant symbols in series" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public async Task Validate_RedundantSymbolsSeries_PassValidation()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                Series = "11 11"
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Series);
        }

        [Fact]
        [Description("Arrange Command come with wrong count of digits in series" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public async Task Validate_WrongCountDigitsSeries_FalseValidationAndErrorMessage()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                Series = "11 11 1",
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Series)
                  .WithErrorMessage(StartRegistrationCommandValidator.SeriesHasWrongFormat);
        }

        [Fact]
        [Description("Arrange Command come with empty series" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public async Task Validate_EmptySeries_FalseValidationAndErrorMessage()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                Series = string.Empty,
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Series)
                  .WithErrorMessage(StartRegistrationCommandValidator.SeriesIsEmpty);
        }

        [Fact]
        [Description("Arrange Command come with regular number" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public async Task Validate_RegularNumber_PassValidation()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                Number = NUMBER
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Number);
        }

        [Fact]
        [Description("Arrange Command come with redundant symbols in number" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public async Task Validate_RedundantSymbolsNumber_PassValidation()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                Number = "111-111"
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Number);
        }

        [Fact]
        [Description("Arrange Command come with wrong count of digits in number" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public async Task Validate_WrongCountDigitsNumber_FalseValidationAndErrorMessage()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                Number = "111 111 1"
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Number)
                  .WithErrorMessage(StartRegistrationCommandValidator.NumberHasWrongFormat);
        }

        [Fact]
        [Description("Arrange Command come with empty number" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public async Task Validate_EmptyNumber_FalseValidationAndErrorMessage()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                Number = string.Empty
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Number)
                  .WithErrorMessage(StartRegistrationCommandValidator.NumberIsEmpty);
        }

        [Fact]
        [Description("Arrange Command come with regular issued by" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public async Task Validate_RegularIssuedBy_PassValidation()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                IssuedBy = ISSUED_BY
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.IssuedBy);
        }

        [Fact]
        [Description("Arrange Command come with empty issued by" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public async Task Validate_EmptyIssuedBy_FalseValidationAndErrorMessage()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                IssuedBy = string.Empty
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.IssuedBy)
                  .WithErrorMessage(StartRegistrationCommandValidator.IssuedByIsEmpty);
        }

        [Fact]
        [Description("Arrange Command come with regular issue id" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public async Task Validate_RegularIssueId_PassValidation()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                IssueId = ISSUE_ID
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.IssueId);
        }

        [Fact]
        [Description("Arrange Command come with redundant symbols in issue id" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public async Task Validate_RedundantSymbolsIssueId_PassValidation()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                IssueId = "111 111"
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.IssueId);
        }

        [Fact]
        [Description("Arrange Command come with wrong count of digits in issue id" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public async Task Validate_WrongCountDigitsIssueId_FalseValidationAndErrorMessage()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                IssueId = "111 111 1"
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.IssueId)
                  .WithErrorMessage(StartRegistrationCommandValidator.IssueIdHasWrongFormat);
        }

        [Fact]
        [Description("Arrange Command come with empty issue id" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public async Task Validate_EmptyIssueId_FalseValidationAndErrorMessage()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                IssueId = string.Empty
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.IssueId)
                  .WithErrorMessage(StartRegistrationCommandValidator.IssueIdIsEmpty);
        }

        [Fact]
        [Description("Arrange Command come with regular citizenship" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public async Task Validate_RegularCitizenship_PassValidation()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                Citizenship = CITIZENSHIP
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Citizenship);
        }

        [Fact]
        [Description("Arrange Command come with empty citizenship" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public async Task Validate_EmptyCitizenship_FalseValidationAndErrorMessage()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                Citizenship = string.Empty
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Citizenship)
                  .WithErrorMessage(StartRegistrationCommandValidator.CitizenshipIsEmpty);
        }

        [Fact]
        [Description("Arrange Command come with regular snils" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public async Task Validate_RegularSnils_PassValidation()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                Snils = SNILS
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Snils);
        }

        [Fact]
        [Description("Arrange Command come with redundant symbols in snils" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public async Task Validate_RedundantSymbolsSnils_PassValidation()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                Snils = "111 111 111 11"
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Snils);
        }

        [Fact]
        [Description("Arrange Command come with wrong count of digits in snils" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public async Task Validate_WrongCountDigitsSnils_FalseValidationAndErrorMessage()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                Snils = "111 111 111 111"
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Snils)
                  .WithErrorMessage(StartRegistrationCommandValidator.SnilsHasWrongFormat);
        }

        [Fact]
        [Description("Arrange Command come with empty snils" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public async Task Validate_EmptySnils_FalseValidationAndErrorMessage()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var validator = new StartRegistrationCommandValidator(dbContext);
            var command = new StartRegistrationCommand
            {
                Snils = string.Empty
            };

            // Act
            var result = await validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Snils)
                  .WithErrorMessage(StartRegistrationCommandValidator.SnilsIsEmpty);
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
