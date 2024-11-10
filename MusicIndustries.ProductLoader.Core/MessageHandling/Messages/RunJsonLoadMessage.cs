using System;
using ProductLoader.DataContracts.SupplierPriceLists.Common;

namespace MusicIndustries.ProductLoader.MessageHandling.Messages
{
    public class RunJsonLoadMessage
    {
        public int RetryCount { get; set; } = 0;
        public TimeSpan NextRetryTime { get; set; } = TimeSpan.Zero;
        public Supplier Supplier { get; set; }

    }
}
