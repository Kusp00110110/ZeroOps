using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ProductLoader.DataContracts.ImageApi
{
    public class ImageApiResponse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Primary key for EF Core.

        // Foreign key for the parent AiOpsProductSchemaResultModel
        [ForeignKey("AiOpsProductSchemaResultModel")]
        public int AiOpsProductSchemaResultModelId { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("image")]
        [MaxLength(1024)]
        public string Image { get; set; }

        [JsonProperty("image_token")]
        [MaxLength(1024)]
        public string ImageToken { get; set; }

        [JsonProperty("source")]
        [MaxLength(1024)]
        public string Source { get; set; }

        [JsonProperty("thumbnail")]
        [MaxLength(1024)]
        public string Thumbnail { get; set; }

        [JsonProperty("thumbnail_token")]
        [MaxLength(1024)]
        public string ThumbnailToken { get; set; }

        [JsonProperty("title")]
        [MaxLength(1024)]
        public string Title { get; set; }

        [JsonProperty("url")]
        [MaxLength(1024)]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

    }
}
