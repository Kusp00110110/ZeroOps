using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using ProductLoader.DataContracts.ImageApi;

namespace ProductLoader.DataContracts.OpenAiApi
{
    public class AiOpsProductSchemaResultModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Primary key for EF Core.

        [MaxLength(1024)]
        public string ItemNumber { get; set; }

        [MaxLength(1024)]
        public string ProductManufacturerBrandName { get; set; }

        [MaxLength(1024)]
        public string ProductName { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string ProductDescription { get; set; }


        [Column(TypeName = "nvarchar(max)")]
        public string ProductCategoriesJson
        {
            get => JsonConvert.SerializeObject(ProductCategoriesAndSubCategories);
            set => ProductCategoriesAndSubCategories = string.IsNullOrEmpty(value)
                ? new List<string>()
                : JsonConvert.DeserializeObject<List<string>>(value);
        }

        [NotMapped]
        public List<string> ProductCategoriesAndSubCategories { get; set; } = new();

        [Column(TypeName = "nvarchar(max)")]
        public string ProductSpecification { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ProductLengthMeters { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ProductWidthMeters { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ProductHeightMeters { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ProductWeightKg { get; set; }

        [MaxLength(500)]
        public string DuckDuckGoImageApiSearchString { get; set; }
    }
    public class AiOpsProductSchemaResultModelDto
    {
        public string ItemNumber { get; set; }

        public string ProductManufacturerBrandName { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public List<string> ProductCategoriesAndSubCategories { get; set; } = new();

        public string ProductSpecification { get; set; }
        public decimal? ProductLengthMeters { get; set; }
        public decimal? ProductWidthMeters { get; set; }
        public decimal? ProductHeightMeters { get; set; }
        public decimal? ProductWeightKg { get; set; }
        public string DuckDuckGoImageApiSearchString { get; set; }
    }


    public class MusicIndustriesLoaderModel
    {
        [Key]
        public int Id { get; set; } // Primary key for EF Core.

        [ForeignKey(nameof(AiOpsProductSchemaResultModelId))]
        public AiOpsProductSchemaResultModel AiOpsProductSchemaResultModel { get; set; }

        public int AiOpsProductSchemaResultModelId { get; set; }

        public List<ImageApiResponse> ImageApiLoaderModelResults { get; set; } = new();
    }
}
