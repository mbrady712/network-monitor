using System;
namespace network_monitor.Models
{
    public class PingResultModel
    {
        public string? IPAddress { get; set; }
        public bool Success { get; set; }
        public string? Status { get; set; }
        public long? RoundTripTime { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime? Timestamp { get; set; }

    }
}
