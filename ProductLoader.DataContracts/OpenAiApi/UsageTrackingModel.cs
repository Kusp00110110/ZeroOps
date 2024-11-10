using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductLoader.DataContracts.OpenAiApi
{
    public class UsageTrackingModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Primary key for EF Core.

        public int InputTokens { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal InputCostPerMillion { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalInputCost { get; set; }

        public int OutputTokens { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal OutputCostPerMillion { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalOutputCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalOutputCostRand { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalInputCostRand { get; set; }
    }
}
