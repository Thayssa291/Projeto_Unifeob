namespace Projeto_Unifeob.Models {
    public class ContribuinteComBeneficios_ViewModel {
        public Contribuinte_Model Contribuinte { get; set; }
        public List<string> Beneficios { get; set; }

        public int RegimeTributacao { get; set; }
    }
}
