using Microsoft.AspNetCore.Mvc;
using Projeto_Unifeob.Data;
using Projeto_Unifeob.Models;

namespace Projeto_Unifeob.Controllers {
    public class BeneficioController : Controller {
        private readonly BancoContext _bancoContext;
        public BeneficioController(BancoContext bancoContext) {
            _bancoContext = bancoContext;
        }
        public IActionResult ListagemBeneficio() {
            List<Beneficio_Model> beneficio = _bancoContext.Beneficio.ToList();
            var BeneficioComContrib = beneficio.Select(b => new BeneficioComContribuinte_ViewModel {
                Beneficio = b,
                Contribuinte = _bancoContext.ContribuinteBeneficio
                    .Where(bc => bc.BeneficioId == b.Id)
                    .Select(bc => bc.Contribuinte.RazaoSocial)
                    .ToList()
            });
            return View(BeneficioComContrib);
        }
        public IActionResult CriarBeneficio() {
            return View();
        }

        public IActionResult EditarBeneficio(int id) {
            var beneficio = _bancoContext.Beneficio.FirstOrDefault(b => b.Id == id);
            return View(beneficio);
        }

        public IActionResult ApagarBeneficio(int id) {
            var beneficio = _bancoContext.Beneficio.FirstOrDefault(b => b.Id == id);
            return View(beneficio);
        }
        public IActionResult Apagar(int id) {
            var beneficioDB = _bancoContext.Beneficio.FirstOrDefault(b => b.Id == id);

            if (beneficioDB == null) {
                ViewBag.MensagemApagar = "Houve um erro ao excluir o benefício";
                return RedirectToAction("ListagemBeneficio");
            }

            // Listagem de contribuinte com o benefício
            var vinculosContribuinte = _bancoContext.ContribuinteBeneficio
                .Where(cb => cb.BeneficioId == id)
                .ToList();

            //Aqui exclui os vinculos:
            _bancoContext.ContribuinteBeneficio.RemoveRange(vinculosContribuinte);
            //Depois remove o benefício:
            _bancoContext.Beneficio.Remove(beneficioDB);
            //Salva no banco:
            _bancoContext.SaveChanges();

            return RedirectToAction("ListagemBeneficio");

        }

        [HttpPost]
        public IActionResult CriarBeneficio(Beneficio_Model beneficio) {
            if (beneficio.nome_beneficio == null) {
                ViewBag.MensagemBeneficio = "Digite um nome para o benefício";
                return View(beneficio);
            }
            if (beneficio.percentual > 100) {
                ViewBag.MensagemPercentual = "O valor do percentual de desconto não pode ser maior que 100%";
                return View(beneficio);
            }
            if (beneficio.percentual <= 0) {
                ViewBag.MensagemPercentual = "Verifique se o valor não é menor ou igual a 0";
                return View(beneficio);
            }
            _bancoContext.Beneficio.Add(beneficio);
            _bancoContext.SaveChanges();

            return RedirectToAction("ListagemBeneficio");
        }

        [HttpPost]
        public IActionResult EditarBeneficio(Beneficio_Model beneficio, int id) {
            var beneficioDB = _bancoContext.Beneficio.FirstOrDefault(b => b.Id == id);

            if (beneficio.nome_beneficio == null) {
                ViewBag.MensagemBeneficio = "Digite um nome para o benefício";
                return View(beneficio);
            }
            if (beneficio.percentual > 100) {
                ViewBag.MensagemPercentual = "O valor do percentual de desconto não pode ser maior que 100%";
                return View(beneficio);
            }
            if (beneficio.percentual <= 0) {
                ViewBag.MensagemPercentual = "Verifique se o valor não é menor ou igual a 0";
                return View(beneficio);

            }
            beneficioDB.nome_beneficio = beneficio.nome_beneficio;
            beneficioDB.percentual = beneficio.percentual;

            _bancoContext.Beneficio.Update(beneficioDB);
            _bancoContext.SaveChanges();
            return RedirectToAction("ListagemBeneficio");
        }
    }
}
