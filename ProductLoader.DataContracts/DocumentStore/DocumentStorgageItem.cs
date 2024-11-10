using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductLoader.DataContracts.DocumentStore
{
    public class DocumentStorgageItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(1024)]
        public string DocumentName { get; set; }


        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string JsonContent { get; set; }
    }
}
