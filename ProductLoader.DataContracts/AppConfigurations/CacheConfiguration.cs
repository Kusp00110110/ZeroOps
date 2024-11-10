using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductLoader.DataContracts.AppConfigurations
{
    public class CacheConfiguration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(1024)]
        [Column(TypeName = "nvarchar(1024)")]
        public string? LocalFileStorageLocation { get; set; }

        [Column(TypeName = "bit")]
        public bool? CreateIfNotExists { get; set; }
    }
}
