using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductLoader.DataContracts.AppConfigurations
{
    public class FilerProviderConfiguration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(1024)]
        public string Root { get; set; }

        [Required]
        [MaxLength(50)]
        public string ImportFolderName { get; set; }

        [Required]
        [MaxLength(50)]
        public string ArchiveFolderName { get; set; }

        [Required]
        [MaxLength(50)]
        public string ErrorFolderName { get; set; }

        [Required]
        public bool CreateIfNotExists { get; set; }
    }
}
