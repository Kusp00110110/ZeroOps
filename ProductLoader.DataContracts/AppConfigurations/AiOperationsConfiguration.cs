using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductLoader.DataContracts.AppConfigurations
{
    public class AiOperationsConfiguration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(1024)]
        [Column(TypeName = "nvarchar(1024)")]
        public string ApiKey { get; set; }

        [Required]
        [MaxLength(1024)]
        [Column(TypeName = "nvarchar(1024)")]
        public string ModelName { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RandToUsdExchangeRate { get; set; }
    }
}
