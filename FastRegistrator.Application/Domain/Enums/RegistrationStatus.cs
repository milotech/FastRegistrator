namespace FastRegistrator.ApplicationCore.Domain.Enums
{
    public enum RegistrationStatus
    {
        // Потенциальный клиент – Заполнил заявление
        PersonDataReceived,
        // Потенциальный клиент – Отправлен на проверку
        PrizmaCheckInProgress,
        // Потенциальный клиент – Проверку не прошел
        PrizmaCheckRejected,
        // Потенциальный клиент – Проверен
        PrizmaCheckSuccessful,
        // Отправлен на регистрацию в ИЦ
        PersonDataSentToIC,
        // Клиент – Счет открыт
        AccountOpened,
        // Регистрация завершена ошибкой
        Error
    }
}
