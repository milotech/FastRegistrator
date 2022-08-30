namespace FastRegistrator.Application.Domain.Enums
{
    public enum RegistrationStatus
    {
        // Потенциальный клиент – Заполнил заявление
        PersonDataReceived = 0,
        // Потенциальный клиент – Отправлен на проверку
        PrizmaCheckInProgress = 1,
        // Потенциальный клиент – Проверку не прошел
        PrizmaCheckRejected = 2,
        // Потенциальный клиент – Проверен
        PrizmaCheckSuccessful = 3,
        // Отправлен на регистрацию в ИЦ
        PersonDataSentToIC = 4,
        // Клиент – Счет открыт
        AccountOpened = 5,
        // Регистрация завершена ошибкой
        Error = 6
    }
}
