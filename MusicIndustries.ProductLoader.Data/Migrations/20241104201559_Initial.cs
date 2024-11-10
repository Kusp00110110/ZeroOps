using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicIndustries.ProductLoader.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AiOperationsConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiKey = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    ModelName = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    RandToUsdExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiOperationsConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AiOpsProductSchemaResultModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemNumber = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    ProductManufacturerBrandName = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    ProductDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCategoriesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductSpecification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductLengthMeters = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProductWidthMeters = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProductHeightMeters = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProductWeightKg = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DuckDuckGoImageApiSearchString = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiOpsProductSchemaResultModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AudioSurePriceListLoadItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MANUFACTURER = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    FormattedItemNumber = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    ItemDescription = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    StockUnitofMeasure = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    UNITPRICE = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    INSTOCKSTATUS = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudioSurePriceListLoadItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CacheConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocalFileStorageLocation = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    CreateIfNotExists = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CacheConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentStoreItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentName = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    JsonContent = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentStoreItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImageApiResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AiOpsProductSchemaResultModelId = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    ImageToken = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Source = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Thumbnail = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    ThumbnailToken = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Width = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageApiResponses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MdPriceListLoadItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    ItemCode = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    ListPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WHCPT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WHGAU = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrandTotal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MdPriceListLoadItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceListLoadItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ItemNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuantityOnHand = table.Column<int>(type: "int", nullable: false),
                    RecommendedRetailPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HasHeader = table.Column<bool>(type: "bit", nullable: false),
                    HeaderRowNumber = table.Column<int>(type: "int", nullable: false),
                    SheetName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeaderStartColumn = table.Column<int>(type: "int", nullable: false),
                    HeaderEndColumn = table.Column<int>(type: "int", nullable: false),
                    SupplierCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceListLoadItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RockitPriceListLoadItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    SRPIncVat = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RockitPriceListLoadItems", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AiOperationsConfiguration");

            migrationBuilder.DropTable(
                name: "AiOpsProductSchemaResultModels");

            migrationBuilder.DropTable(
                name: "AudioSurePriceListLoadItems");

            migrationBuilder.DropTable(
                name: "CacheConfiguration");

            migrationBuilder.DropTable(
                name: "DocumentStoreItems");

            migrationBuilder.DropTable(
                name: "ImageApiResponses");

            migrationBuilder.DropTable(
                name: "MdPriceListLoadItems");

            migrationBuilder.DropTable(
                name: "PriceListLoadItems");

            migrationBuilder.DropTable(
                name: "RockitPriceListLoadItems");
        }
    }
}
