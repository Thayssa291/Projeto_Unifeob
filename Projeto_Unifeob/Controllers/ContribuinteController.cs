using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto_Unifeob.Data;
using Projeto_Unifeob.Models;
using System.Globalization;
using System.Linq.Expressions;

namespace Projeto_Unifeob.Controllers {
    public class ContribuinteController : Controller { // Herança
        private readonly BancoContext _bancoContext;
        public ContribuinteController(BancoContext bancoContext) {
            _bancoContext = bancoContext;
        }
        public IActionResult EditarContrib(int id) {
            var contribuinte = _bancoContext.Contribuinte.FirstOrDefault(c => c.Id == id); //Abstração
            var beneficiosVinculados = _bancoContext.ContribuinteBeneficio
            .Where(cb => cb.ContribuinteId == id)
            .Select(cb => cb.BeneficioId.ToString())
            .ToList();

            ViewBag.BeneficiosVinculados = beneficiosVinculados;
            ListaRegimes();
            ListaBeneficio();
            return View(contribuinte);
        }

        public IActionResult ApagarConfirmacao(int id) {
            var contribuinte = _bancoContext.Contribuinte.FirstOrDefault(c => c.Id == id);
            return View(contribuinte);
        }

        public IActionResult Apagar(int id) {
            var contribuinte = _bancoContext.Contribuinte.FirstOrDefault(c => c.Id == id);

            if (contribuinte == null) {
                ViewBag.MensagemApagar = "Houve um erro ao excluir o contribuinte";
            }

            //Busca os vínculos de contribuinte com benefício:
            var vinculosBeneficio = _bancoContext.ContribuinteBeneficio
                .Where(cb => cb.ContribuinteId == id)
                .ToList();

            // Aqui vai excluir os vínculos:
            _bancoContext.ContribuinteBeneficio.RemoveRange(vinculosBeneficio);

            _bancoContext.Contribuinte.Remove(contribuinte);

            _bancoContext.SaveChanges();

            return RedirectToAction("ListagemContrib");
        }

        public IActionResult ListagemContrib() {

            List<Contribuinte_Model> contribuinte = _bancoContext.Contribuinte.ToList();

            // Cria a lista de ContribuinteComBeneficios_ViewModel
            var contribuinteComBeneficios = contribuinte.Select(c => new ContribuinteComBeneficios_ViewModel {
                Contribuinte = c,
                Beneficios = _bancoContext.ContribuinteBeneficio
                    .Where(cb => cb.ContribuinteId == c.Id)
                    .Select(cb => cb.Beneficio.nome_beneficio)
                    .ToList(),
                RegimeTributacao = c.RegimeTributacao
            }).ToList();


            return View(contribuinteComBeneficios);
        }

        public IActionResult CriarContrib() {
            ListaRegimes();
            ListaBeneficio();

            return View();
        }

        [HttpPost]
        public IActionResult CriarContrib(Contribuinte_Model contribuinte, List<int> BeneficioId) {
            ListaRegimes();
            ListaBeneficio();

            //Tira as formatações do CNPJ para salvar no banco:
            if (!string.IsNullOrEmpty(contribuinte.Cnpj)) {
                contribuinte.Cnpj = contribuinte.Cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
            }

            //Validação se o CNPJ não é nullo
            if (contribuinte.Cnpj == null) {
                ViewBag.MensagemCnpj = "Digite um CNPJ";
            }

            if (string.IsNullOrEmpty(contribuinte.Cnpj) || contribuinte.Cnpj.Length < 14) {
                ViewBag.MensagemCnpj = "Verifique se o CNPJ possuí 14 caracteres";
                return View(contribuinte);
            }
            //Validação para que não crie um contrib com o mesmo CNPJ:
            bool cnpjJaCadastrado = _bancoContext.Contribuinte
            .Any(c => c.Cnpj == contribuinte.Cnpj);
            if (cnpjJaCadastrado == true) {
                ViewBag.MensagemCnpj = "CNPJ já cadastrado";
                return View(contribuinte);
            }
            //Validação se a Razão social não é nula
            if (contribuinte.RazaoSocial == null) {
                ViewBag.MensagemRazao = "Digite uma Razão Social";
                return View(contribuinte);
            }
            //Validação da data de abertura:
            if (contribuinte.DataAbertura == DateTime.MinValue || contribuinte.DataAbertura > DateTime.Now) {
                ViewBag.MensagemData = "A data de abertura não pode ser nula ou maior que a data atual.";
                return View(contribuinte);
            }

            //Validação do regime:
            if (contribuinte.RegimeTributacao == 0) {
                ViewBag.MensagemRegime = "Informe um Regime de Tributação";
                return View(contribuinte);
            }

            // Salvando no banco
            _bancoContext.Contribuinte.Add(contribuinte);
            _bancoContext.SaveChanges();

            // Vincular os benefícios selecionados ao contribuinte
            foreach (var beneficioId in BeneficioId) {
                var contribuinteBeneficio = new ContribuinteBeneficio_Model {
                    ContribuinteId = contribuinte.Id,
                    BeneficioId = beneficioId
                };
                _bancoContext.ContribuinteBeneficio.Add(contribuinteBeneficio);
            }
            _bancoContext.SaveChanges();
            return RedirectToAction("ListagemContrib");
        }
        [HttpPost]
        public IActionResult EditarContrib(Contribuinte_Model contribuinte, int id, List<int> BeneficioId) {

            var contribuinteDB = _bancoContext.Contribuinte.FirstOrDefault(c => c.Id == id);

            if (contribuinteDB == null) {
                ViewBag.MensagemAtualizacao = "Houve um erro na atualização do contribuinte.";
            }

            var beneficiosVinculados = _bancoContext.ContribuinteBeneficio
            .Where(cb => cb.ContribuinteId == id)
            .Select(cb => cb.BeneficioId.ToString())
            .ToList();

            ViewBag.BeneficiosVinculados = beneficiosVinculados;

            ListaBeneficio();
            ListaRegimes();

            //Tira as formatações do CNPJ para salvar no banco:
            if (!string.IsNullOrEmpty(contribuinte.Cnpj)) {
                contribuinte.Cnpj = contribuinte.Cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
            }

            //Validação se o CNPJ não é nullo
            if (contribuinte.Cnpj == null) {
                ViewBag.MensagemCnpjNull = "Digite um CNPJ";
            }

            if (string.IsNullOrEmpty(contribuinte.Cnpj) || contribuinte.Cnpj.Length < 14) {
                ViewBag.MensagemCnpjInvalido = "O CNPJ deve ter exatamente 14 caracteres";
                return View(contribuinte);
            }

            //Validação para que não crie um contrib com o mesmo CNPJ:
            bool cnpjJaCadastrado = _bancoContext.Contribuinte
            .Any(c => c.Cnpj == contribuinte.Cnpj && c.Id != id); // Exclui o ID do contribuinte atual
            if (cnpjJaCadastrado) {
                ViewBag.MensagemCnpj = "CNPJ já cadastrado para outro contribuinte.";
                return View(contribuinte);
            }
            //Validação se a Razão social não é nula
            if (contribuinte.RazaoSocial == null) {
                ViewBag.MensagemRazao = "Digite uma Razão Social.";
                return View(contribuinte);
            }

            //Validação da data de abertura:
            if (contribuinte.DataAbertura == DateTime.MinValue || contribuinte.DataAbertura > DateTime.Now) {
                ViewBag.MensagemData = "A data de abertura não pode ser nula ou maior que a data atual.";
                return View(contribuinte);
            }

            if (contribuinte.RegimeTributacao == 0) {
                ViewBag.MensagemRegime = "Informe um Regime de Tributação";
                return View(contribuinte);
            }

            contribuinteDB.Cnpj = contribuinte.Cnpj;
            contribuinteDB.RazaoSocial = contribuinte.RazaoSocial;
            contribuinteDB.DataAbertura = contribuinte.DataAbertura;
            contribuinteDB.RegimeTributacao = contribuinte.RegimeTributacao;

            _bancoContext.Contribuinte.Update(contribuinteDB);
            _bancoContext.SaveChanges();

            //Vai atualizar os beneficios que estão cadastrados caso seja alterado:
            var beneficiosAtuais = _bancoContext.ContribuinteBeneficio
                .Where(cb => cb.ContribuinteId == contribuinteDB.Id)
                .ToList();
            // Remove benefícios antigos
            _bancoContext.ContribuinteBeneficio.RemoveRange(beneficiosAtuais);
            _bancoContext.SaveChanges();

            foreach (var beneficioId in BeneficioId) {
                var contribuinteBeneficio = new ContribuinteBeneficio_Model {
                    ContribuinteId = contribuinte.Id,
                    BeneficioId = beneficioId
                };
                _bancoContext.ContribuinteBeneficio.Add(contribuinteBeneficio);
            }
            _bancoContext.SaveChanges();
            return RedirectToAction("ListagemContrib");
        }

        private IActionResult ListaRegimes() {
            var regimes = new List<SelectListItem> {
            new SelectListItem { Text = "Microempresa municipal", Value = "1" },
            new SelectListItem { Text = "Estimativa", Value = "2" },
            new SelectListItem { Text = "Sociedade de profissionais", Value = "3" },
            new SelectListItem { Text = "Cooperativa", Value = "4" },
            new SelectListItem { Text = "Microempresário Individual (MEI)", Value = "5" },
            new SelectListItem { Text = "Microempresário e Empresa de Pequeno Porte (ME EPP)", Value = "6" },
            new SelectListItem { Text = "Tributação por Faturamento (Variável)", Value = "7" },
            new SelectListItem { Text = "Fixo", Value = "8" },
            new SelectListItem { Text = "Isenção", Value = "9" },
            new SelectListItem { Text = "Imune", Value = "10" },
            new SelectListItem { Text = "Exigibilidade suspensa por decisão judicial", Value = "11" },
            new SelectListItem { Text = "Exigibilidade Suspensa por Procedimento Administrativo", Value = "12" }
    };
            ViewBag.Regimes = regimes;
            return View();
        }
        private IActionResult ListaBeneficio() {
            var beneficios = _bancoContext.Beneficio.ToList();
            var beneficiosList = beneficios.Select(b => new SelectListItem {
                Value = b.Id.ToString(),
                Text = b.nome_beneficio,
            }).ToList();
            ViewBag.Beneficios = beneficiosList;
            return View();
        }
    }
}