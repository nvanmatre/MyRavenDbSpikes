using System;

namespace FunWithIncludes.Tests.Services.Entities
{
    public class Notification
    {
        public string CreatedBy { get; set; }
        public string CustomerId { get; set; }
        public string Id { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }

        public string TimestampFormat
        {
            get
            {
                return Timestamp.ToLocalTime().ToString("MMM dd, h:mm") +
                       Timestamp.ToLocalTime().ToString("tt").ToLower();
            }
        }
    }
}