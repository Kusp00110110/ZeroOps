using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace ProductLoader.DataContracts.SupplierPriceLists.Common
{
    public class PriceListRow : IExcelModel
    {
        // Primary Key
        [Key] // Marks this property as the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Automatically generates a unique value
        public int Id { get; set; }

        // Backing field for the Brand property
        private string brand;

        [Required] // Ensures that Brand cannot be null
        [MaxLength(100)] // Limits the length of Brand to 100 characters
        public string Brand
        {
            get => ToTitleCase(brand);
            set => brand = ToTitleCase(value);
        }

        [Required] // Marks this property as mandatory
        [MaxLength(50)] // Sets a limit on the length of the item number
        public string ItemNumber { get; set; }

        public string Description { get; set; } // Optional field for product description

        public int QuantityOnHand { get; set; } // Tracks available inventory

        [Column(TypeName = "decimal(18, 2)")] // Ensures precision in SQL for decimal values
        public decimal RecommendedRetailPrice { get; set; }

        // Helper method to convert a string to title case
        private string ToTitleCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(input.ToLower());
        }

        public bool HasHeader { get; set; } // Flag indicating if the Excel file has a header row

        public ExcelRowEnums HeaderRowNumber { get; set; } // Enum for row number of the header

        public string SheetName { get; set; } // Name of the Excel sheet

        public ExcelColumnEnums HeaderStartColumn { get; set; } // Start column of header in Excel

        public ExcelColumnEnums HeaderEndColumn { get; set; } // End column of header in Excel

        [MaxLength(50)] // Limit supplier codes to 10 characters
        public string SupplierCode { get; set; }


        public void MapColumn(string columnName, string columnValue)
        {
            throw new System.NotImplementedException();
        }

        public PriceListRow MapToPriceListRow()
        {
            return this;
        }
    }
}
