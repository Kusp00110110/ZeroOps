#nullable enable
using System;

namespace ProductLoader.DataContracts.NopCommerce
{
    public class AdvancedProduct
    {
        public int? ProductId { get; set; }

        public string? ProductType { get; set; }

        public int? ParentGroupedProductId { get; set; }

        public bool? VisibleIndividually { get; set; }

        public string? Name { get; set; }

        public string? ShortDescription { get; set; }

        public string? FullDescription { get; set; }

        public string? Vendor { get; set; }

        public string? ProductTemplate { get; set; }

        public bool? ShowOnHomepage { get; set; }

        public int? DisplayOrder { get; set; }

        public string? MetaKeywords { get; set; }

        public string? MetaDescription { get; set; }

        public string? MetaTitle { get; set; }

        public string? SeName { get; set; }

        public bool? AllowCustomerReviews { get; set; }

        public bool? Published { get; set; }

        public string? SKU { get; set; }

        public string? ManufacturerPartNumber { get; set; }

        public string? Gtin { get; set; }

        public bool? IsGiftCard { get; set; }

        public string? GiftCardType { get; set; }

        public decimal? OverriddenGiftCardAmount { get; set; }

        public bool? RequireOtherProducts { get; set; }

        public string? RequiredProductIds { get; set; }

        public bool? AutomaticallyAddRequiredProducts { get; set; }

        public bool? IsDownload { get; set; }

        public int? DownloadId { get; set; }

        public bool? UnlimitedDownloads { get; set; }

        public int? MaxNumberOfDownloads { get; set; }

        public string? DownloadActivationType { get; set; }

        public bool? HasSampleDownload { get; set; }

        public int? SampleDownloadId { get; set; }

        public bool? HasUserAgreement { get; set; }

        public string? UserAgreementText { get; set; }

        public bool? IsRecurring { get; set; }

        public int? RecurringCycleLength { get; set; }

        public string? RecurringCyclePeriod { get; set; }

        public int? RecurringTotalCycles { get; set; }

        public bool? IsRental { get; set; }

        public int? RentalPriceLength { get; set; }

        public string? RentalPricePeriod { get; set; }

        public bool? IsShipEnabled { get; set; }

        public bool? IsFreeShipping { get; set; }

        public bool? ShipSeparately { get; set; }

        public decimal? AdditionalShippingCharge { get; set; }

        public string? DeliveryDate { get; set; }

        public bool? IsTaxExempt { get; set; }

        public string? TaxCategory { get; set; }

        public string? ManageInventoryMethod { get; set; }

        public string? ProductAvailabilityRange { get; set; }

        public bool? UseMultipleWarehouses { get; set; }

        public int? WarehouseId { get; set; }

        public int? StockQuantity { get; set; }

        public bool? DisplayStockAvailability { get; set; }

        public bool? DisplayStockQuantity { get; set; }

        public int? MinStockQuantity { get; set; }

        public string? LowStockActivity { get; set; }

        public int? NotifyAdminForQuantityBelow { get; set; }

        public string? BackorderMode { get; set; }

        public bool? AllowBackInStockSubscriptions { get; set; }

        public int? OrderMinimumQuantity { get; set; }

        public int? OrderMaximumQuantity { get; set; }

        public string? AllowedQuantities { get; set; }

        public bool? AllowAddingOnlyExistingAttributeCombinations { get; set; }

        public bool? NotReturnable { get; set; }

        public bool? DisableBuyButton { get; set; }

        public bool? DisableWishlistButton { get; set; }

        public bool? AvailableForPreOrder { get; set; }

        public DateTime? PreOrderAvailabilityStartDateTimeUtc { get; set; }

        public bool? CallForPrice { get; set; }

        public decimal? Price { get; set; }

        public decimal? OldPrice { get; set; }

        public decimal? ProductCost { get; set; }

        public bool? CustomerEntersPrice { get; set; }

        public decimal? MinimumCustomerEnteredPrice { get; set; }

        public decimal? MaximumCustomerEnteredPrice { get; set; }

        public bool? BasepriceEnabled { get; set; }

        public decimal? BasepriceAmount { get; set; }

        public string? BasepriceUnit { get; set; }

        public decimal? BasepriceBaseAmount { get; set; }

        public string? BasepriceBaseUnit { get; set; }

        public bool? MarkAsNew { get; set; }

        public DateTime? MarkAsNewStartDateTimeUtc { get; set; }

        public DateTime? MarkAsNewEndDateTimeUtc { get; set; }

        public decimal? Weight { get; set; }

        public decimal? Length { get; set; }

        public decimal? Width { get; set; }

        public decimal? Height { get; set; }

        public string? Categories { get; set; }

        public string? Manufacturers { get; set; }

        public string? ProductTags { get; set; }

        public string? Picture1 { get; set; }

        public string? Picture2 { get; set; }

        public string? Picture3 { get; set; }
    }

    public static class ProductExtensions
    {
        public static AdvancedProduct ApplyDefaults(
            this SimpleProduct simpleLoad)
        {
            var product = new AdvancedProduct()
            {
                ProductId = null,
                ProductType = "Simple Product",
                ParentGroupedProductId = 0,
                VisibleIndividually = true,
                Name = simpleLoad.Name,
                ShortDescription = simpleLoad.ShortDescription,
                FullDescription = simpleLoad.FullDescription,
                Vendor = null,
                ProductTemplate = null,
                ShowOnHomepage = false,
                DisplayOrder = 0,
                MetaKeywords = null,
                MetaDescription = null,
                MetaTitle = null,
                SeName = null,
                AllowCustomerReviews = false,
                Published = true,
                SKU = simpleLoad.SKU,
                ManufacturerPartNumber = null,
                Gtin = null,
                IsGiftCard = false,
                GiftCardType = null,
                OverriddenGiftCardAmount = null,
                RequireOtherProducts = false,
                RequiredProductIds = null,
                AutomaticallyAddRequiredProducts = false,
                IsDownload = false,
                DownloadId = 0,
                UnlimitedDownloads = false,
                MaxNumberOfDownloads = 0,
                DownloadActivationType = null,
                HasSampleDownload = false,
                SampleDownloadId = 0,
                HasUserAgreement = false,
                UserAgreementText = null,
                IsRecurring = false,
                RecurringCycleLength = 0,
                RecurringCyclePeriod = null,
                RecurringTotalCycles = 0,
                IsRental = false,
                RentalPriceLength = 0,
                RentalPricePeriod = null,
                IsShipEnabled = true,
                IsFreeShipping = false,
                ShipSeparately = false,
                AdditionalShippingCharge = 0,
                DeliveryDate = null,
                IsTaxExempt = false,
                TaxCategory = null,
                ManageInventoryMethod = "Manage Stock",
                ProductAvailabilityRange = null,
                UseMultipleWarehouses = false,
                WarehouseId = 1,
                StockQuantity = 1,
                DisplayStockAvailability = false,
                DisplayStockQuantity = false,
                MinStockQuantity = 0,
                LowStockActivity = "Nothing",
                NotifyAdminForQuantityBelow = 0,
                BackorderMode = "No Backorders",
                AllowBackInStockSubscriptions = false,
                OrderMinimumQuantity = 0,
                OrderMaximumQuantity = 0,
                AllowedQuantities = null,
                AllowAddingOnlyExistingAttributeCombinations = false,
                NotReturnable = false,
                DisableBuyButton = false,
                DisableWishlistButton = false,
                AvailableForPreOrder = false,
                PreOrderAvailabilityStartDateTimeUtc = null,
                CallForPrice = false,
                Price = simpleLoad.Price,
                OldPrice = 0.0000m,
                ProductCost = 0.0000m,
                CustomerEntersPrice = false,
                MinimumCustomerEnteredPrice = 0.0000m,
                MaximumCustomerEnteredPrice = 0.0000m,
                BasepriceEnabled = false,
                BasepriceAmount = 0.0000m,
                BasepriceUnit = null,
                BasepriceBaseAmount = 0.0000m,
                BasepriceBaseUnit = null,
                MarkAsNew = true,
                MarkAsNewStartDateTimeUtc = DateTime.UtcNow,
                MarkAsNewEndDateTimeUtc = DateTime.UtcNow.AddDays(7),
                Weight = simpleLoad.Weight,
                Length = simpleLoad.Length,
                Width = simpleLoad.Width,
                Height = simpleLoad.Height,
                Categories = simpleLoad.Categories,
                Manufacturers = simpleLoad.Manufacturers,
                ProductTags = null,
                Picture1 = simpleLoad.Picture1,
                Picture2 = simpleLoad.Picture2,
                Picture3 = simpleLoad.Picture3
            };

            return product;
        }
    }
}
