namespace FastRegistrator.Application.DTOs.PrizmaService
{
    public record PersonCheckError(
        string Message,
        int PrizmaErrorCode,
        Dictionary<string, List<string>>? Errors);
}
