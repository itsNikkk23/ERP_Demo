using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LoginForm.Models
{
    [Table("Weaving_process", Schema = "Manufacture")]
    public class Weaving_process
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WeaveID { get; set; }

        [Required]
        [StringLength(30)]
        public string weave_type { get; set; }

        [Required]
        //[StringLength(255)]
        public string loomType { get; set; }

    }
}
