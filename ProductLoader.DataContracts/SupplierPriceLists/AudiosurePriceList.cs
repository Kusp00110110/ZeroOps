using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using ProductLoader.DataContracts.SupplierPriceLists.Common;

namespace ProductLoader.DataContracts.SupplierPriceLists
{
    public class AudioSurePriceList : IExcelModel
    {
        /*  "Brand": "Akai",
    "ItemNumber": "MPC KEY 37",
    "Description": "Music Production Centre with 37Keys,16 Pads and Touch Screen",
    "QuantityOnHand": 0,
    "RecommendedRetailPrice": 26995.0,
    "HasHeader": true,
    "HeaderRowNumber": 5,
    "SheetName": "SOH",
    "HeaderStartColumn": 1,
    "HeaderEndColumn": 6,
    "SupplierCode": "AUDIOSURE"*/

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // EF Core requires a primary key.

        [Required]
        [MaxLength(1024)]
        [JsonProperty("Brand")]
        public string MANUFACTURER { get; set; }

        [MaxLength(1024)]
        [JsonProperty("ItemNumber")]
        public string FormattedItemNumber { get; set; }

        [MaxLength(1024)]
        [JsonProperty("Description")]
        public string ItemDescription { get; set; }

        [MaxLength(1024)]
        [JsonProperty("Category")]
        public string Category { get; set; }

        [MaxLength(1024)]
        public string StockUnitofMeasure { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UNITPRICE { get; set; }

        [MaxLength(1024)]
        public string INSTOCKSTATUS { get; set; }

        [NotMapped] // This property won’t be stored in the database.
        public int StockQuantity
        {
            get
            {
                if (string.IsNullOrEmpty(INSTOCKSTATUS))
                    return 0;

                return INSTOCKSTATUS.ToLower() switch
                {
                    "10 or more in stock" => 10,
                    "less than 10 in stock" => 1,
                    "out of stock" => 0,
                    _ => 0
                };
            }
        }

        [NotMapped]
        public bool HasHeader { get; set; } = true;

        [NotMapped]
        public ExcelRowEnums HeaderRowNumber { get; set; } = ExcelRowEnums.Five;

        [NotMapped]
        public string SheetName { get; set; } = "SOH";

        [NotMapped]
        public ExcelColumnEnums HeaderStartColumn { get; set; } = ExcelColumnEnums.A;

        [NotMapped]
        public ExcelColumnEnums HeaderEndColumn { get; set; } = ExcelColumnEnums.F;

        [NotMapped]
        public string SupplierCode { get; set; } = "AUDIOSURE";

        public void MapColumn(string columnName, string columnValue)
        {
            switch (columnName)
            {
                case nameof(MANUFACTURER):
                    MANUFACTURER = columnValue;
                    break;
                case nameof(FormattedItemNumber):
                    var itemNumber = columnValue.Split('-').TakeLast(1).FirstOrDefault()?.Trim();
                    FormattedItemNumber = itemNumber;
                    break;
                case nameof(ItemDescription):
                    ItemDescription = columnValue;
                    break;
                case nameof(Category):
                    Category = "";
                    break;
                case nameof(StockUnitofMeasure):
                    StockUnitofMeasure = columnValue;
                    break;
                case nameof(UNITPRICE):
                    UNITPRICE = decimal.TryParse(columnValue, out var price) ? price : 0;
                    break;
                case nameof(INSTOCKSTATUS):
                    INSTOCKSTATUS = columnValue;
                    break;
            }
        }

        public PriceListRow MapToPriceListRow()
        {
            return new PriceListRow
            {
                Brand = MANUFACTURER,
                ItemNumber = FormattedItemNumber,
                Description = ItemDescription,
                QuantityOnHand = StockQuantity,
                RecommendedRetailPrice = UNITPRICE,
                HasHeader = HasHeader,
                HeaderRowNumber = HeaderRowNumber,
                SheetName = SheetName,
                HeaderStartColumn = HeaderStartColumn,
                HeaderEndColumn = HeaderEndColumn,
                SupplierCode = SupplierCode
            };
        }
    }
}
