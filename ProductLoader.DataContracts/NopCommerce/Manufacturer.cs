#nullable enable
namespace ProductLoader.DataContracts.NopCommerce
{
    public class Manufacturer
    {
        public class NopCommerceManufacturerLoadModel
        {
            public string? Id { get; set; }

            public string? Name { get; set; }

            public string? Description { get; set; }

            public string? ManufacturerTemplateId { get; set; }

            public string? Picture { get; set; }

            public string? DisplayOrder { get; set; }

        }
    }
}
