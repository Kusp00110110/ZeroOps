using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProductLoader.DataContracts.SupplierPriceLists.Common;

namespace ProductLoader.DataContracts.SupplierPriceLists
{
    public class MdPriceList : IExcelModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Primary key required by EF Core.

        [Required]
        [MaxLength(1024)]
        [Column(TypeName = "nvarchar(1024)")]
        public string Brand { get; set; }

        [Required]
        [MaxLength(1024)]
        [Column(TypeName = "nvarchar(1024)")]
        public string ItemCode { get; set; }

        [MaxLength(1024)]
        [Column(TypeName = "nvarchar(1024)")]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ListPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal WHCPT { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal WHGAU { get; set; }

        [Column(TypeName = "int")]
        public int GrandTotal { get; set; }

        [NotMapped]
        public bool HasHeader { get; set; } = true;

        [NotMapped]
        public ExcelRowEnums HeaderRowNumber { get; set; } = ExcelRowEnums.One;

        [NotMapped]
        public string SheetName { get; set; } = "Sheet1";

        [NotMapped]
        public ExcelColumnEnums HeaderStartColumn { get; set; } = ExcelColumnEnums.A;

        [NotMapped]
        public ExcelColumnEnums HeaderEndColumn { get; set; } = ExcelColumnEnums.G;

        [NotMapped]
        public string SupplierCode { get; set; } = "MUSICALDISTRIBUTORS";

        public void MapColumn(string columnName, string columnValue)
        {
            switch (columnName)
            {
                case nameof(Brand):
                    Brand = columnValue;
                    break;
                case nameof(ItemCode):
                    ItemCode = columnValue;
                    break;
                case nameof(Description):
                    Description = columnValue;
                    break;
                case nameof(ListPrice):
                    ListPrice = decimal.TryParse(columnValue, out var price) ? price : 0;
                    break;
                case nameof(WHCPT):
                    WHCPT = decimal.TryParse(columnValue, out var cptStock) ? cptStock : 0;
                    break;
                case nameof(WHGAU):
                    WHGAU = decimal.TryParse(columnValue, out var gauStock) ? gauStock : 0;
                    break;
                case nameof(GrandTotal):
                    GrandTotal = int.TryParse(columnValue, out var totalStock) ? totalStock : 0;
                    break;
            }
        }

        public PriceListRow MapToPriceListRow()
        {
            return new PriceListRow
            {
                Brand = Brand,
                ItemNumber = ItemCode,
                Description = Description,
                QuantityOnHand = GrandTotal,
                RecommendedRetailPrice = ListPrice,
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
