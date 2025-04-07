using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto_Unifeob.Models {
    public class Beneficio_Model {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string nome_beneficio { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal percentual { get; set; }

        //Relacionamentos:
        public ICollection<ContribuinteBeneficio_Model> ContribuinteBeneficios { get; set; } = new List<ContribuinteBeneficio_Model>();
    }
}
