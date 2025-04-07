using Projeto_Unifeob.Controllers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_Unifeob.Models {
    [Table("Contribuinte")]
    public class Contribuinte_Model {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string RazaoSocial { get; set; }

        [Required]
        [StringLength(14)]
        public string Cnpj { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DataAbertura { get; set; }
        [Required]
        public int RegimeTributacao { get; set; }

        //Relacionamento:
        public ICollection<ContribuinteBeneficio_Model> ContribuinteBeneficios { get; set; } = new List<ContribuinteBeneficio_Model>();

    }

}
