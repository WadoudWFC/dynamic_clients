using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MutipleHttpClient.Domain.Shared
{
    public class ErrorResponse
    {
        public string Code { get; set; } = "UNKNOWN_ERROR";
        public string Message { get; set; } = "An unexpected error occurred";
        public string? Details { get; set; }
        public string? StackTrace { get; set; }
        public string? InnerException { get; set; }
        public string? SupportMessage { get; set; }
        public string? CorrelationId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
