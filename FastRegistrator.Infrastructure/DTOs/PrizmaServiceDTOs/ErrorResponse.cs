namespace FastRegistrator.Infrastructure.DTOs.PrizmaServiceDTOs
{
    public record ErrorResponse(
        string Message,
        int PrizmaErrorCode,
        Dictionary<string, List<string>>? Errors);
}
