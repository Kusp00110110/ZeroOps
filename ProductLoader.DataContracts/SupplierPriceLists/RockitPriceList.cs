using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProductLoader.DataContracts.SupplierPriceLists.Common;

namespace ProductLoader.DataContracts.SupplierPriceLists
{
    public class RockitPriceList : IExcelModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Primary key for EF Core.

        [Required]
        [MaxLength(1024)]
        [Column(TypeName = "nvarchar(1024)")]
        public string Brand { get; set; }

        [Required]
        [MaxLength(1024)]
        [Column(TypeName = "nvarchar(1024)")]
        public string Code { get; set; }

        [MaxLength(1024)]
        [Column(TypeName = "nvarchar(1024)")]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SRPIncVat { get; set; }

        [NotMapped]
        public bool HasHeader { get; set; } = true;

        [NotMapped]
        public ExcelRowEnums HeaderRowNumber { get; set; } = ExcelRowEnums.Fifteen;

        [NotMapped]
        public string SheetName { get; set; } = "Sheet1";

        [NotMapped]
        public ExcelColumnEnums HeaderStartColumn { get; set; } = ExcelColumnEnums.A;

        [NotMapped]
        public ExcelColumnEnums HeaderEndColumn { get; set; } = ExcelColumnEnums.D;

        [NotMapped]
        public string SupplierCode { get; set; } = "ROCKIT";

        public void MapColumn(string columnName, string columnValue)
        {

            switch (columnName)
            {
                case nameof(Code):
                    Code = columnValue;
                    break;
                case nameof(Brand):
                    Brand = columnValue;
                    break;
                case nameof(Description):
                    Description = columnValue;
                    break;
                case nameof(SRPIncVat):
                    SRPIncVat = decimal.TryParse(columnValue, out var price) ? price : 0;
                    break;
            }
        }

        public PriceListRow MapToPriceListRow()
        {
            return new PriceListRow
            {
                Brand = Brand,
                ItemNumber = Code,
                Description = Description,
                QuantityOnHand = 1,
                RecommendedRetailPrice = SRPIncVat,
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
