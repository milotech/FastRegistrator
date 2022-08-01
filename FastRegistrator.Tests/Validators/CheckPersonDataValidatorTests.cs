using FastRegistrator.ApplicationCore.Commands.SetStatusESIAApproved;
using FastRegistrator.Tests.Commands;
using FluentValidation.TestHelper;
using System.ComponentModel;

namespace FastRegistrator.Tests.Validators
{
    public class CheckPersonDataValidatorTests
    {
        [Fact]
        [Description("Arrange Command come with regular first name" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public void Validate_RegularFirstName_PassValidation()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = SetStatusESIAApprovedCommandHandlerTest.FIRST_NAME,
                LastName = SetStatusESIAApprovedCommandHandlerTest.LAST_NAME,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = SetStatusESIAApprovedCommandHandlerTest.SERIES,
                Number = SetStatusESIAApprovedCommandHandlerTest.NUMBER,
                IssuedBy = SetStatusESIAApprovedCommandHandlerTest.ISSUED_BY,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = SetStatusESIAApprovedCommandHandlerTest.ISSUE_ID,
                Citizenship = SetStatusESIAApprovedCommandHandlerTest.CITIZENSHIP,
                Snils = SetStatusESIAApprovedCommandHandlerTest.SNILS
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        [Description("Arrange Command come with empty first name" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public void Validate_EmptyFirstName_FalseValidationAndErrorMessage()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = string.Empty,
                LastName = SetStatusESIAApprovedCommandHandlerTest.LAST_NAME,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = SetStatusESIAApprovedCommandHandlerTest.SERIES,
                Number = SetStatusESIAApprovedCommandHandlerTest.NUMBER,
                IssuedBy = SetStatusESIAApprovedCommandHandlerTest.ISSUED_BY,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = SetStatusESIAApprovedCommandHandlerTest.ISSUE_ID,
                Citizenship = SetStatusESIAApprovedCommandHandlerTest.CITIZENSHIP,
                Snils = SetStatusESIAApprovedCommandHandlerTest.SNILS
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                  .WithErrorMessage(SetStatusESIAApprovedCommandValidator.FirstNameIsEmpty);
        }

        [Fact]
        [Description("Arrange Command come with regular last name" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public void Validate_RegularLastName_PassValidation()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = SetStatusESIAApprovedCommandHandlerTest.FIRST_NAME,
                LastName = SetStatusESIAApprovedCommandHandlerTest.LAST_NAME,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = SetStatusESIAApprovedCommandHandlerTest.SERIES,
                Number = SetStatusESIAApprovedCommandHandlerTest.NUMBER,
                IssuedBy = SetStatusESIAApprovedCommandHandlerTest.ISSUED_BY,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = SetStatusESIAApprovedCommandHandlerTest.ISSUE_ID,
                Citizenship = SetStatusESIAApprovedCommandHandlerTest.CITIZENSHIP,
                Snils = SetStatusESIAApprovedCommandHandlerTest.SNILS
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        [Description("Arrange Command come with empty last name" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public void Validate_EmptyLastName_FalseValidationAndErrorMessage()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = SetStatusESIAApprovedCommandHandlerTest.FIRST_NAME,
                LastName = string.Empty,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = SetStatusESIAApprovedCommandHandlerTest.SERIES,
                Number = SetStatusESIAApprovedCommandHandlerTest.NUMBER,
                IssuedBy = SetStatusESIAApprovedCommandHandlerTest.ISSUED_BY,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = SetStatusESIAApprovedCommandHandlerTest.ISSUE_ID,
                Citizenship = SetStatusESIAApprovedCommandHandlerTest.CITIZENSHIP,
                Snils = SetStatusESIAApprovedCommandHandlerTest.SNILS
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                  .WithErrorMessage(SetStatusESIAApprovedCommandValidator.LastNameIsEmpty);
        }

        [Fact]
        [Description("Arrange Command come with regular series" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public void Validate_RegularSeries_PassValidation()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = SetStatusESIAApprovedCommandHandlerTest.FIRST_NAME,
                LastName = SetStatusESIAApprovedCommandHandlerTest.LAST_NAME,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = SetStatusESIAApprovedCommandHandlerTest.SERIES,
                Number = SetStatusESIAApprovedCommandHandlerTest.NUMBER,
                IssuedBy = SetStatusESIAApprovedCommandHandlerTest.ISSUED_BY,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = SetStatusESIAApprovedCommandHandlerTest.ISSUE_ID,
                Citizenship = SetStatusESIAApprovedCommandHandlerTest.CITIZENSHIP,
                Snils = SetStatusESIAApprovedCommandHandlerTest.SNILS
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Series);
        }

        [Fact]
        [Description("Arrange Command come with redundant symbols in series" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public void Validate_RedundantSymbolsSeries_PassValidation()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = SetStatusESIAApprovedCommandHandlerTest.FIRST_NAME,
                LastName = SetStatusESIAApprovedCommandHandlerTest.LAST_NAME,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = "11 11",
                Number = SetStatusESIAApprovedCommandHandlerTest.NUMBER,
                IssuedBy = SetStatusESIAApprovedCommandHandlerTest.ISSUED_BY,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = SetStatusESIAApprovedCommandHandlerTest.ISSUE_ID,
                Citizenship = SetStatusESIAApprovedCommandHandlerTest.CITIZENSHIP,
                Snils = SetStatusESIAApprovedCommandHandlerTest.SNILS
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Series);
        }

        [Fact]
        [Description("Arrange Command come with wrong count of digits in series" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public void Validate_WrongCountDigitsSeries_FalseValidationAndErrorMessage()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = SetStatusESIAApprovedCommandHandlerTest.FIRST_NAME,
                LastName = SetStatusESIAApprovedCommandHandlerTest.LAST_NAME,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = "11 11 1",
                Number = SetStatusESIAApprovedCommandHandlerTest.NUMBER,
                IssuedBy = SetStatusESIAApprovedCommandHandlerTest.ISSUED_BY,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = SetStatusESIAApprovedCommandHandlerTest.ISSUE_ID,
                Citizenship = SetStatusESIAApprovedCommandHandlerTest.CITIZENSHIP,
                Snils = SetStatusESIAApprovedCommandHandlerTest.SNILS
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Series)
                  .WithErrorMessage(SetStatusESIAApprovedCommandValidator.SeriesHasWrongFormat);
        }

        [Fact]
        [Description("Arrange Command come with empty series" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public void Validate_EmptySeries_FalseValidationAndErrorMessage()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = SetStatusESIAApprovedCommandHandlerTest.FIRST_NAME,
                LastName = SetStatusESIAApprovedCommandHandlerTest.LAST_NAME,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = string.Empty,
                Number = SetStatusESIAApprovedCommandHandlerTest.NUMBER,
                IssuedBy = SetStatusESIAApprovedCommandHandlerTest.ISSUED_BY,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = SetStatusESIAApprovedCommandHandlerTest.ISSUE_ID,
                Citizenship = SetStatusESIAApprovedCommandHandlerTest.CITIZENSHIP,
                Snils = SetStatusESIAApprovedCommandHandlerTest.SNILS
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Series)
                  .WithErrorMessage(SetStatusESIAApprovedCommandValidator.SeriesIsEmpty);
        }

        [Fact]
        [Description("Arrange Command come with regular number" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public void Validate_RegularNumber_PassValidation()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = SetStatusESIAApprovedCommandHandlerTest.FIRST_NAME,
                LastName = SetStatusESIAApprovedCommandHandlerTest.LAST_NAME,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = SetStatusESIAApprovedCommandHandlerTest.SERIES,
                Number = SetStatusESIAApprovedCommandHandlerTest.NUMBER,
                IssuedBy = SetStatusESIAApprovedCommandHandlerTest.ISSUED_BY,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = SetStatusESIAApprovedCommandHandlerTest.ISSUE_ID,
                Citizenship = SetStatusESIAApprovedCommandHandlerTest.CITIZENSHIP,
                Snils = SetStatusESIAApprovedCommandHandlerTest.SNILS
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Number);
        }

        [Fact]
        [Description("Arrange Command come with redundant symbols in number" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public void Validate_RedundantSymbolsNumber_PassValidation()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = SetStatusESIAApprovedCommandHandlerTest.FIRST_NAME,
                LastName = SetStatusESIAApprovedCommandHandlerTest.LAST_NAME,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = SetStatusESIAApprovedCommandHandlerTest.SERIES,
                Number = "111-111",
                IssuedBy = SetStatusESIAApprovedCommandHandlerTest.ISSUED_BY,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = SetStatusESIAApprovedCommandHandlerTest.ISSUE_ID,
                Citizenship = SetStatusESIAApprovedCommandHandlerTest.CITIZENSHIP,
                Snils = SetStatusESIAApprovedCommandHandlerTest.SNILS
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Number);
        }

        [Fact]
        [Description("Arrange Command come with wrong count of digits in number" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public void Validate_WrongCountDigitsNumber_FalseValidationAndErrorMessage()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = SetStatusESIAApprovedCommandHandlerTest.FIRST_NAME,
                LastName = SetStatusESIAApprovedCommandHandlerTest.LAST_NAME,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = SetStatusESIAApprovedCommandHandlerTest.SERIES,
                Number = "111 111 1",
                IssuedBy = SetStatusESIAApprovedCommandHandlerTest.ISSUED_BY,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = SetStatusESIAApprovedCommandHandlerTest.ISSUE_ID,
                Citizenship = SetStatusESIAApprovedCommandHandlerTest.CITIZENSHIP,
                Snils = SetStatusESIAApprovedCommandHandlerTest.SNILS
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Number)
                  .WithErrorMessage(SetStatusESIAApprovedCommandValidator.NumberHasWrongFormat);
        }

        [Fact]
        [Description("Arrange Command come with empty number" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public void Validate_EmptyNumber_FalseValidationAndErrorMessage()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = SetStatusESIAApprovedCommandHandlerTest.FIRST_NAME,
                LastName = SetStatusESIAApprovedCommandHandlerTest.LAST_NAME,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = SetStatusESIAApprovedCommandHandlerTest.SERIES,
                Number = string.Empty,
                IssuedBy = SetStatusESIAApprovedCommandHandlerTest.ISSUED_BY,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = SetStatusESIAApprovedCommandHandlerTest.ISSUE_ID,
                Citizenship = SetStatusESIAApprovedCommandHandlerTest.CITIZENSHIP,
                Snils = SetStatusESIAApprovedCommandHandlerTest.SNILS
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Number)
                  .WithErrorMessage(SetStatusESIAApprovedCommandValidator.NumberIsEmpty);
        }

        [Fact]
        [Description("Arrange Command come with regular issued by" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public void Validate_RegularIssuedBy_PassValidation()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = SetStatusESIAApprovedCommandHandlerTest.FIRST_NAME,
                LastName = SetStatusESIAApprovedCommandHandlerTest.LAST_NAME,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = SetStatusESIAApprovedCommandHandlerTest.SERIES,
                Number = SetStatusESIAApprovedCommandHandlerTest.NUMBER,
                IssuedBy = SetStatusESIAApprovedCommandHandlerTest.ISSUED_BY,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = SetStatusESIAApprovedCommandHandlerTest.ISSUE_ID,
                Citizenship = SetStatusESIAApprovedCommandHandlerTest.CITIZENSHIP,
                Snils = SetStatusESIAApprovedCommandHandlerTest.SNILS
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.IssuedBy);
        }

        [Fact]
        [Description("Arrange Command come with empty issued by" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public void Validate_EmptyIssuedBy_FalseValidationAndErrorMessage()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = SetStatusESIAApprovedCommandHandlerTest.FIRST_NAME,
                LastName = SetStatusESIAApprovedCommandHandlerTest.LAST_NAME,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = SetStatusESIAApprovedCommandHandlerTest.SERIES,
                Number = SetStatusESIAApprovedCommandHandlerTest.NUMBER,
                IssuedBy = string.Empty,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = SetStatusESIAApprovedCommandHandlerTest.ISSUE_ID,
                Citizenship = SetStatusESIAApprovedCommandHandlerTest.CITIZENSHIP,
                Snils = SetStatusESIAApprovedCommandHandlerTest.SNILS
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.IssuedBy)
                  .WithErrorMessage(SetStatusESIAApprovedCommandValidator.IssuedByIsEmpty);
        }

        [Fact]
        [Description("Arrange Command come with regular issue id" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public void Validate_RegularIssueId_PassValidation()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = SetStatusESIAApprovedCommandHandlerTest.FIRST_NAME,
                LastName = SetStatusESIAApprovedCommandHandlerTest.LAST_NAME,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = SetStatusESIAApprovedCommandHandlerTest.SERIES,
                Number = SetStatusESIAApprovedCommandHandlerTest.NUMBER,
                IssuedBy = SetStatusESIAApprovedCommandHandlerTest.ISSUED_BY,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = SetStatusESIAApprovedCommandHandlerTest.ISSUE_ID,
                Citizenship = SetStatusESIAApprovedCommandHandlerTest.CITIZENSHIP,
                Snils = SetStatusESIAApprovedCommandHandlerTest.SNILS
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.IssueId);
        }

        [Fact]
        [Description("Arrange Command come with redundant symbols in issue id" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public void Validate_RedundantSymbolsIssueId_PassValidation()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = SetStatusESIAApprovedCommandHandlerTest.FIRST_NAME,
                LastName = SetStatusESIAApprovedCommandHandlerTest.LAST_NAME,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = SetStatusESIAApprovedCommandHandlerTest.SERIES,
                Number = SetStatusESIAApprovedCommandHandlerTest.NUMBER,
                IssuedBy = SetStatusESIAApprovedCommandHandlerTest.ISSUED_BY,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = "111 111",
                Citizenship = SetStatusESIAApprovedCommandHandlerTest.CITIZENSHIP,
                Snils = SetStatusESIAApprovedCommandHandlerTest.SNILS
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.IssueId);
        }

        [Fact]
        [Description("Arrange Command come with wrong count of digits in issue id" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public void Validate_WrongCountDigitsIssueId_FalseValidationAndErrorMessage()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = SetStatusESIAApprovedCommandHandlerTest.FIRST_NAME,
                LastName = SetStatusESIAApprovedCommandHandlerTest.LAST_NAME,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = SetStatusESIAApprovedCommandHandlerTest.SERIES,
                Number = SetStatusESIAApprovedCommandHandlerTest.NUMBER,
                IssuedBy = SetStatusESIAApprovedCommandHandlerTest.ISSUED_BY,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = "111 111 1",
                Citizenship = SetStatusESIAApprovedCommandHandlerTest.CITIZENSHIP,
                Snils = SetStatusESIAApprovedCommandHandlerTest.SNILS
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.IssueId)
                  .WithErrorMessage(SetStatusESIAApprovedCommandValidator.IssueIdHasWrongFormat);
        }

        [Fact]
        [Description("Arrange Command come with empty issue id" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public void Validate_EmptyIssueId_FalseValidationAndErrorMessage()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = SetStatusESIAApprovedCommandHandlerTest.FIRST_NAME,
                LastName = SetStatusESIAApprovedCommandHandlerTest.LAST_NAME,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = SetStatusESIAApprovedCommandHandlerTest.SERIES,
                Number = SetStatusESIAApprovedCommandHandlerTest.NUMBER,
                IssuedBy = SetStatusESIAApprovedCommandHandlerTest.ISSUED_BY,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = string.Empty,
                Citizenship = SetStatusESIAApprovedCommandHandlerTest.CITIZENSHIP,
                Snils = SetStatusESIAApprovedCommandHandlerTest.SNILS
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.IssueId)
                  .WithErrorMessage(SetStatusESIAApprovedCommandValidator.IssueIdIsEmpty);
        }

        [Fact]
        [Description("Arrange Command come with regular citizenship" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public void Validate_RegularCitizenship_PassValidation()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = SetStatusESIAApprovedCommandHandlerTest.FIRST_NAME,
                LastName = SetStatusESIAApprovedCommandHandlerTest.LAST_NAME,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = SetStatusESIAApprovedCommandHandlerTest.SERIES,
                Number = SetStatusESIAApprovedCommandHandlerTest.NUMBER,
                IssuedBy = SetStatusESIAApprovedCommandHandlerTest.ISSUED_BY,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = SetStatusESIAApprovedCommandHandlerTest.ISSUE_ID,
                Citizenship = SetStatusESIAApprovedCommandHandlerTest.CITIZENSHIP,
                Snils = SetStatusESIAApprovedCommandHandlerTest.SNILS
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Citizenship);
        }

        [Fact]
        [Description("Arrange Command come with empty citizenship" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public void Validate_EmptyCitizenship_FalseValidationAndErrorMessage()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = SetStatusESIAApprovedCommandHandlerTest.FIRST_NAME,
                LastName = SetStatusESIAApprovedCommandHandlerTest.LAST_NAME,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = SetStatusESIAApprovedCommandHandlerTest.SERIES,
                Number = SetStatusESIAApprovedCommandHandlerTest.NUMBER,
                IssuedBy = SetStatusESIAApprovedCommandHandlerTest.ISSUED_BY,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = SetStatusESIAApprovedCommandHandlerTest.ISSUE_ID,
                Citizenship = string.Empty,
                Snils = SetStatusESIAApprovedCommandHandlerTest.SNILS
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Citizenship)
                  .WithErrorMessage(SetStatusESIAApprovedCommandValidator.CitizenshipIsEmpty);
        }

        [Fact]
        [Description("Arrange Command come with regular snils" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public void Validate_RegularSnils_PassValidation()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = SetStatusESIAApprovedCommandHandlerTest.FIRST_NAME,
                LastName = SetStatusESIAApprovedCommandHandlerTest.LAST_NAME,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = SetStatusESIAApprovedCommandHandlerTest.SERIES,
                Number = SetStatusESIAApprovedCommandHandlerTest.NUMBER,
                IssuedBy = SetStatusESIAApprovedCommandHandlerTest.ISSUED_BY,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = SetStatusESIAApprovedCommandHandlerTest.ISSUE_ID,
                Citizenship = SetStatusESIAApprovedCommandHandlerTest.CITIZENSHIP,
                Snils = SetStatusESIAApprovedCommandHandlerTest.SNILS
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Snils);
        }

        [Fact]
        [Description("Arrange Command come with redundant symbols in snils" +
                     "Act Validate method is called" +
                     "Assert Validation result is true")]
        public void Validate_RedundantSymbolsSnils_PassValidation()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = SetStatusESIAApprovedCommandHandlerTest.FIRST_NAME,
                LastName = SetStatusESIAApprovedCommandHandlerTest.LAST_NAME,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = SetStatusESIAApprovedCommandHandlerTest.SERIES,
                Number = SetStatusESIAApprovedCommandHandlerTest.NUMBER,
                IssuedBy = SetStatusESIAApprovedCommandHandlerTest.ISSUED_BY,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = SetStatusESIAApprovedCommandHandlerTest.ISSUE_ID,
                Citizenship = SetStatusESIAApprovedCommandHandlerTest.CITIZENSHIP,
                Snils = "111 111 111 11"
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Snils);
        }

        [Fact]
        [Description("Arrange Command come with wrong count of digits in snils" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public void Validate_WrongCountDigitsSnils_FalseValidationAndErrorMessage()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = SetStatusESIAApprovedCommandHandlerTest.FIRST_NAME,
                LastName = SetStatusESIAApprovedCommandHandlerTest.LAST_NAME,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = SetStatusESIAApprovedCommandHandlerTest.SERIES,
                Number = SetStatusESIAApprovedCommandHandlerTest.NUMBER,
                IssuedBy = SetStatusESIAApprovedCommandHandlerTest.ISSUED_BY,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = SetStatusESIAApprovedCommandHandlerTest.ISSUE_ID,
                Citizenship = SetStatusESIAApprovedCommandHandlerTest.CITIZENSHIP,
                Snils = "111 111 111 111"
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Snils)
                  .WithErrorMessage(SetStatusESIAApprovedCommandValidator.SnilsHasWrongFormat);
        }

        [Fact]
        [Description("Arrange Command come with empty snils" +
                     "Act Validate method is called" +
                     "Assert Validation result is false and it has appropriate error message")]
        public void Validate_EmptySnils_FalseValidationAndErrorMessage()
        {
            // Arrange
            var validator = new SetStatusESIAApprovedCommandValidator();
            var command = new SetStatusESIAApprovedCommand
            {
                PhoneNumber = SetStatusESIAApprovedCommandHandlerTest.PERSON_PHONE_NUMBER,
                FirstName = SetStatusESIAApprovedCommandHandlerTest.FIRST_NAME,
                LastName = SetStatusESIAApprovedCommandHandlerTest.LAST_NAME,
                MiddleName = SetStatusESIAApprovedCommandHandlerTest.MIDDLE_NAME,
                Series = SetStatusESIAApprovedCommandHandlerTest.SERIES,
                Number = SetStatusESIAApprovedCommandHandlerTest.NUMBER,
                IssuedBy = SetStatusESIAApprovedCommandHandlerTest.ISSUED_BY,
                IssueDate = SetStatusESIAApprovedCommandHandlerTest.ISSUE_DATE,
                IssueId = SetStatusESIAApprovedCommandHandlerTest.ISSUE_ID,
                Citizenship = SetStatusESIAApprovedCommandHandlerTest.CITIZENSHIP,
                Snils = string.Empty
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Snils)
                  .WithErrorMessage(SetStatusESIAApprovedCommandValidator.SnilsIsEmpty);
        }
    }
}
