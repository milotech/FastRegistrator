using FastRegistrator.ApplicationCore.Domain.Entities;

namespace FastRegistrator.ApplicationCore.DTOs.GetStatusDTOs
{
    public class GetStatusResponse
    {
        public StatusHistoryItem? StatusHistoryItem { get; set; }
        public Exception? Exception { get; set; }

        public GetStatusResponse(StatusHistoryItem? statusHistoryItem, Exception? exception)
        {
            StatusHistoryItem = statusHistoryItem;
            Exception = exception;
        }
    }
}