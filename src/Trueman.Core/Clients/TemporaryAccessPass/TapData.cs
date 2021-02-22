using System;

namespace Trueman.Core.Clients.TemporaryAccessPass
{
    public class TapData
    {
        public string Id { get; set; }
        public string TemporaryAccessPass { get; set; }
        public bool IsUsable { get; set; }
        public bool IsUsableOnce { get; set; }
        public int LifetimeInMinutes { get; set; }
        public DateTimeOffset StartDateTime { get; set; }
    }
}
