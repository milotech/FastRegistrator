﻿namespace FastRegistrator.ApplicationCore.Domain.Enums
{
    public enum RegistrationStatus
    {
        // Потенциальный клиент – Заполнил заявление
        ClientFilledApplication,
        // Потенциальный клиент – Отправлен на проверку
        PrizmaCheckInProgress,
        // Потенциальный клиент – Проверку не прошел
        PrizmaCheckRejected,
        // Потенциальный клиент – Проверен
        PrizmaCheckSuccessful,
        // Потенциальный клиент – Ошибка со стороны Призмы
        PrizmaCheckFailed,
        // Отправлен на регистрацию в ИЦ
        ClientSentForRegistrationToIC,
        // Клиент – Счет открыт
        AccountOpened,
        // Клиент – Счет закрыт
        AccountClosed
    }
}