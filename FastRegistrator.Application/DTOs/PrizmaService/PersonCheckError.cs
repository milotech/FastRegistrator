namespace FastRegistrator.ApplicationCore.DTOs.PrizmaService
{
    public record PersonCheckError(
        string Message,
        int PrizmaErrorCode,
        Dictionary<string, List<string>>? Errors);
}
