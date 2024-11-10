using System;
using ProductLoader.DataContracts.SupplierPriceLists.Common;

namespace MusicIndustries.ProductLoader.MessageHandling.Messages
{
    public class LoadSupplierPriceList
    {
        public Supplier Supplier { get; set; }

        public int retryCount { get; set; }

        public TimeSpan nextRetryTime { get; set; }
    }
}
