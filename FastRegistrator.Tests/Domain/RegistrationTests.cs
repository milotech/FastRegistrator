using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.Domain.Enums;
using FastRegistrator.ApplicationCore.Domain.Events;
using FastRegistrator.ApplicationCore.Domain.ValueObjects;
using System.ComponentModel;
using static FastRegistrator.Tests.Constants;

namespace FastRegistrator.Tests.Domain
{
    public class RegistrationTests
    {
        [Fact]
        [Description("Arrange Create new registration object" +
                     "Act Registration constructor is called" +
                     "Assert Registration has status 'PersonDataReceived' and domain event 'RegistrationStartedEvent'")]
        public void Registration_NewRegistrationObject_RegistrationHasStatusPersonDataReceived()
        {
            // Arrange & Act
            var registration = new Registration(GUID, PHONE_NUMBER, ConstructPersonData());

            // Assert
            var assertStatus = registration.StatusHistory.OrderByDescending(x => x.StatusDT).First().Status;

            Assert.Equal(RegistrationStatus.PersonDataReceived, assertStatus);
            Assert.Contains(new RegistrationStartedEvent(registration), registration.DomainEvents);
        }

        [Fact]
        [Description("Arrange Created registration with Completed flag is true (e.g. registration finished with error)" +
                     "Act Call method SetPrizmaCheckInProgress for exsisting registraion" +
                     "Assert Registration throws InvalidOperationException")]
        public void SetPrizmaCheckInProgress_CallMethodSetPrizmaCheckInProgressWithCompletedIsTrue_RegistrationThrowsInvalidOperationException()
        {
            // Arrange
            var registration = new Registration(GUID, PHONE_NUMBER, ConstructPersonData());
            var error = new Error(ErrorSource.FastRegistrator, "some error");
            registration.SetError(error);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => registration.SetPrizmaCheckInProgress());
        }

        [Fact]
        [Description("Arrange Created registration" +
                     "Act Call method SetPrizmaCheckInProgress for exsisting registraion" +
                     "Assert Registration has status 'PrizmaCheckInProgress'")]
        public void SetPrizmaCheckInProgress_CallMethodSetPrizmaCheckInProgress_RegistrationHasStatusPrizmaCheckInProgress()
        {
            // Arrange
            var registration = new Registration(GUID, PHONE_NUMBER, ConstructPersonData());

            // Act
            registration.SetPrizmaCheckInProgress();

            // Assert
            var assertStatus = registration.StatusHistory.OrderByDescending(x => x.StatusDT).First().Status;

            Assert.Equal(RegistrationStatus.PrizmaCheckInProgress, assertStatus);
        }

        [Fact]
        [Description("Arrange Created registration with Completed flag is true (e.g. registration finished with error)" +
                     "Act Call method SetPrizmaCheckResult for exsisting registraion" +
                     "Assert Registration throws InvalidOperationException")]
        public void SetPrizmaCheckResult_CallMethodSetPrizmaCheckResult_RegistrationThrowsInvalidOperationException()
        {
            // Arrange
            var registration = new Registration(GUID, PHONE_NUMBER, ConstructPersonData());
            var error = new Error(ErrorSource.FastRegistrator, "some error");
            registration.SetError(error);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => registration.SetPrizmaCheckResult(ConstructPositivPrizmaCheckResult()));
        }

        [Fact]
        [Description("Arrange Created registration" +
                     "Act Call method SetPrizmaCheckResult for exsisting registraion" +
                     "Assert PrizmaCheckResult is not null")]
        public void SetPrizmaCheckResult_CallMethodSetPrizmaCheckResult_PrizmaCheckResultIsNotNull()
        {
            // Arrange
            var registration = new Registration(GUID, PHONE_NUMBER, ConstructPersonData());

            // Act
            registration.SetPrizmaCheckResult(ConstructPositivPrizmaCheckResult());

            // Assert
            Assert.NotNull(registration.PrizmaCheckResult);
        }

        [Fact]
        [Description("Arrange Created registration" +
                     "Act Call method SetPrizmaCheckResult for exsisting registraion with positiv PrizmaCheckResult" +
                     "Assert Registration has status 'PrizmaCheckSuccessful' and domain event 'PrizmaCheckPassedEvent'")]
        public void SetPrizmaCheckResult_CallMethodSetPrizmaCheckResultWithPositivPrizmaCheckResult_RegistrationHasStatusPrizmaCheckSuccessful()
        {
            // Arrange
            var registration = new Registration(GUID, PHONE_NUMBER, ConstructPersonData());

            // Act
            registration.SetPrizmaCheckResult(ConstructPositivPrizmaCheckResult());

            // Assert
            var assertStatus = registration.StatusHistory.OrderByDescending(x => x.StatusDT).First().Status;

            Assert.Equal(RegistrationStatus.PrizmaCheckSuccessful, assertStatus);
            Assert.Contains(new PrizmaCheckPassedEvent(registration), registration.DomainEvents);
        }

        [Fact]
        [Description("Arrange Created registration" +
                     "Act Call method SetPrizmaCheckResult for exsisting registraion with negativ PrizmaCheckResult" +
                     "Assert Registration has status 'PrizmaCheckRejected' and Complete flag is true'")]
        public void SetPrizmaCheckResult_CallMethodSetPrizmaCheckResultWithNegativPrizmaCheckResult_RegistrationHasStatusPrizmaCheckRejected()
        {
            // Arrange
            var registration = new Registration(GUID, PHONE_NUMBER, ConstructPersonData());

            // Act
            registration.SetPrizmaCheckResult(ConstructNegativPrizmaCheckResult());

            // Assert
            var assertStatus = registration.StatusHistory.OrderByDescending(x => x.StatusDT).First().Status;

            Assert.Equal(RegistrationStatus.PrizmaCheckRejected, assertStatus);
            Assert.True(registration.Completed);
            Assert.Contains(new RegistrationCompletedEvent(registration), registration.DomainEvents);
        }

        [Fact]
        [Description("Arrange Created registration with Completed flag is true (e.g. registration finished with error)" +
                     "Act Call method SetError for exsisting registraion" +
                     "Assert Registration throws InvalidOperationException")]
        public void SetError_CallMethodSetError_RegistrationThrowsInvalidOperationException()
        {
            // Arrange
            var registration = new Registration(GUID, PHONE_NUMBER, ConstructPersonData());
            var error = new Error(ErrorSource.FastRegistrator, "some error");
            registration.SetError(error);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => registration.SetError(error));
        }

        [Fact]
        [Description("Arrange Created registration" +
                     "Act Call method SetError for exsisting registraion" +
                     "Assert Error is not null")]
        public void SetError_CallMethodSetPrizmaCheckResult_ErrorIsNotNull()
        {
            // Arrange
            var registration = new Registration(GUID, PHONE_NUMBER, ConstructPersonData());
            var error = new Error(ErrorSource.FastRegistrator, "some error");

            // Act
            registration.SetError(error);

            // Assert
            Assert.NotNull(registration.Error);
        }

        [Fact]
        [Description("Arrange Created registration" +
                     "Act Call method SetError for exsisting registraion" +
                     "Assert Registration has Complete flag is true and domain event 'RegistrationCompletedEvent'")]
        public void SetError_CallMethodSetError_RegistrationHasCompleteFlagIsTrue()
        {
            // Arrange
            var registration = new Registration(GUID, PHONE_NUMBER, ConstructPersonData());
            var error = new Error(ErrorSource.FastRegistrator, "some error");

            // Act
            registration.SetError(error);

            // Assert
            Assert.True(registration.Completed);
            Assert.Contains(new RegistrationCompletedEvent(registration), registration.DomainEvents);
        }

        [Fact]
        [Description("Arrange Created registration" +
                     "Act Call method SetError for exsisting registraion" +
                     "Assert Registration has status 'Error'")]
        public void SetError_CallMethodSetError_RegistrationHasStatusError()
        {
            // Arrange
            var registration = new Registration(GUID, PHONE_NUMBER, ConstructPersonData());
            var error = new Error(ErrorSource.FastRegistrator, "some error");

            // Act
            registration.SetError(error);

            // Assert
            var assertStatus = registration.StatusHistory.OrderByDescending(x => x.StatusDT).First().Status;

            Assert.Equal(RegistrationStatus.Error, assertStatus);
        }

        [Fact]
        [Description("Arrange Created registration with Completed flag is true (e.g. registration finished with error)" +
                     "Act Call method SetPersonDataSentToIC for exsisting registraion" +
                     "Assert Registration throws InvalidOperationException")]
        public void SetPersonDataSentToIC_CallMethodSetPersonDataSentToIC_RegistrationThrowsInvalidOperationException()
        {
            // Arrange
            var registration = new Registration(GUID, PHONE_NUMBER, ConstructPersonData());
            var error = new Error(ErrorSource.FastRegistrator, "some error");
            registration.SetError(error);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => registration.SetPersonDataSentToIC());
        }

        [Fact]
        [Description("Arrange Created registration" +
                     "Act Call method SetPersonDataSentToIC for exsisting registraion" +
                     "Assert Registration has status 'PersonDataSentToIC'")]
        public void SetPersonDataSentToIC_CallMethodSetPersonDataSentToIC_RegistrationHasStatusPersonDataSentToIC()
        {
            // Arrange
            var registration = new Registration(GUID, PHONE_NUMBER, ConstructPersonData());

            // Act
            registration.SetPersonDataSentToIC();

            // Assert
            var assertStatus = registration.StatusHistory.OrderByDescending(x => x.StatusDT).First().Status;

            Assert.Equal(RegistrationStatus.PersonDataSentToIC, assertStatus);
        }

        [Fact]
        [Description("Arrange Created registration with Completed flag is true (e.g. registration finished with error)" +
                     "Act Call method SetAccountOpened for exsisting registraion" +
                     "Assert Registration throws InvalidOperationException")]
        public void SetAccountOpened_CallMethodSetAccountOpened_RegistrationThrowsInvalidOperationException()
        {
            // Arrange
            var registration = new Registration(GUID, PHONE_NUMBER, ConstructPersonData());
            var error = new Error(ErrorSource.FastRegistrator, "some error");
            registration.SetError(error);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => registration.SetAccountOpened());
        }

        [Fact]
        [Description("Arrange Created registration" +
                     "Act Call method SetAccountOpened for exsisting registraion" +
                     "Assert Registration has Complete flag is true and domain event 'RegistrationCompletedEvent'")]
        public void SetAccountOpened_CallMethodSetAccountOpened_RegistrationHasCompleteFlagIsTrue()
        {
            // Arrange
            var registration = new Registration(GUID, PHONE_NUMBER, ConstructPersonData());

            // Act
            registration.SetAccountOpened();

            // Assert
            Assert.True(registration.Completed);
            Assert.Contains(new RegistrationCompletedEvent(registration), registration.DomainEvents);
        }

        [Fact]
        [Description("Arrange Created registration" +
                     "Act Call method SetAccountOpened for exsisting registraion" +
                     "Assert Registration has status 'AccountOpened'")]
        public void SetAccountOpened_CallMethodSetAccountOpened_RegistrationHasStatusAccountOpened()
        {
            // Arrange
            var registration = new Registration(GUID, PHONE_NUMBER, ConstructPersonData());

            // Act
            registration.SetAccountOpened();

            // Assert
            var assertStatus = registration.StatusHistory.OrderByDescending(x => x.StatusDT).First().Status;

            Assert.Equal(RegistrationStatus.AccountOpened, assertStatus);
        }

        private PersonData ConstructPersonData()
        {
            var personName = new PersonName(FIRST_NAME, MIDDLE_NAME, LAST_NAME);
            var personData = new PersonData(personName, PHONE_NUMBER, PASSPORT_NUMBER, BIRTHDAY, INN, FORM_DATA);

            return personData;
        }

        private PrizmaCheckResult ConstructPositivPrizmaCheckResult()
            => new PrizmaCheckResult(RejectionReason.None, "Prizma reponse");

        private PrizmaCheckResult ConstructNegativPrizmaCheckResult()
            => new PrizmaCheckResult(RejectionReason.BlackListed, "Prizma reponse");
    }
}
