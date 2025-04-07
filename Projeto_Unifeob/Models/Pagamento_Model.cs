using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Projeto_Unifeob.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Projeto_Unifeob.Models {
    public class Pagamento_Model {
        public int Id { get; set; }
        public decimal valor_inicial { get; set; }
        public decimal valor_total { get; set; }
        public decimal Desconto { get; set; }
        public int ContribuinteId { get; set; }
        public Contribuinte_Model Contribuinte { get; set; }
        public int? BeneficioId { get; set; }
        public Beneficio_Model Beneficio { get; set; }

    }
}

