namespace FastRegistrator.ApplicationCore.DTOs.PrizmaServiceDTOs
{
    public record ErrorResponse(
        string Message,
        int PrizmaErrorCode,
        Dictionary<string, List<string>>? Errors);
}
