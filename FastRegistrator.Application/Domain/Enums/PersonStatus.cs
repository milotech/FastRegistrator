namespace FastRegistrator.ApplicationCore.Domain.Enums
{
    public enum PersonStatus
    {
        // Не подтверждена учетная запись ЕСИА
        ESIANotApproved,
        // Подтверждена учетная запись ЕСИА
        ESIAApproved,
        // Отказ клиента от регистрации
        PersonRejected,
        // Потенциальный клиент – Заполнил заявление
        ClientFilledApplication,
        // Потенциальный клиент – Отправлен на проверку
        PrizmaCheckInProgress,
        // Потенциальный клиент – Проверку не прошел
        PrizmaCheckRejected,
        // Потенциальный клиент – Проверен
        PrizmaCheckSuccessful,
        // Потенциальный клиент – Готов к регистрации в учетной системе
        ClientReadyForRegistration,
        // Клиент – Счет открыт
        AccountOpened,
        // Клиент – Счет закрыт
        AccountClosed
    }
}
