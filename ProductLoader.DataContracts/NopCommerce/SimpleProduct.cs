#nullable enable
namespace ProductLoader.DataContracts.NopCommerce
{
    public class SimpleProduct
    {
        public string? ProductId { get; set; }

        public string? Name { get; set; }

        public string? ShortDescription { get; set; }

        public string? FullDescription { get; set; }

        public string? SKU { get; set; }

        public bool IsShipEnabled { get; set; } = true;

        public bool IsTaxExempt { get; set; } = false;

        public string? TaxCategory { get; set; }

        public string? ManageInventoryMethod { get; set; } = "Manage Stock";

        public int StockQuantity { get; set; } = 0;

        public decimal? Price { get; set; }

        public decimal? Weight { get; set; } = 1m;

        public decimal? Length { get; set; } = 1m;

        public decimal? Width { get; set; } = 1m;

        public decimal? Height { get; set; } = 1m;

        public string? Categories { get; set; }

        public string? Manufacturers { get; set; }

        public string? Picture1 { get; set; }

        public string? Picture2 { get; set; }

        public string? Picture3 { get; set; }
    }
}
