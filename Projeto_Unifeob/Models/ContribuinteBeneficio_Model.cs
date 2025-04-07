using Projeto_Unifeob.Controllers;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Projeto_Unifeob.Models {
    public class ContribuinteBeneficio_Model {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Contribuinte")]
        public int ContribuinteId { get; set; }
        public Contribuinte_Model Contribuinte { get; set; }

        [ForeignKey("Beneficio")]
        public int BeneficioId { get; set; }
        public Beneficio_Model Beneficio { get; set; }
    }
}
