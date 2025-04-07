using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto_Unifeob.Data;
using Projeto_Unifeob.Models;

namespace Projeto_Unifeob.Controllers {
    public class Pagamento : Controller {
        private readonly BancoContext _bancoContext;
        public Pagamento(BancoContext bancoContext) {
            _bancoContext = bancoContext;
        }

        public IActionResult Simular() {
            var contribuinte = _bancoContext.Contribuinte.ToList();

            var contribuinteList = contribuinte.Select(c => new SelectListItem {
                Value = c.Id.ToString(),
                Text = $"{c.RazaoSocial} - CNPJ: {c.Cnpj}"  // Adicionando o CNPJ ao texto
            }).ToList();

            var model = new Pagamento_Model();
            ViewBag.Contribuintes = contribuinteList;

            return View(model);
        }

        [HttpPost]
        public IActionResult Simular(Pagamento_Model pagamento) {
            var contribuinte = _bancoContext.Contribuinte
                .FirstOrDefault(c => c.Id == pagamento.ContribuinteId);

            var contribuinteList = _bancoContext.Contribuinte.Select(c => new SelectListItem {
                Value = c.Id.ToString(),
                Text = c.RazaoSocial
            }).ToList();

            ViewBag.Contribuintes = contribuinteList;

            if (contribuinte == null) {
                ViewBag.MensagemCnpj = "Selecione um contribuinte";
                return View(pagamento);
            }
            if (pagamento.valor_inicial <= 0 || pagamento.valor_inicial == null) {
                ViewBag.MensagemValor = "Verifique se o valor é nulo ou menor que 0";
                return View(pagamento);
            }
            // Carregar os benefícios do CNPJ selecionado e preenchê-los no modelo
            var beneficiosList = _bancoContext.ContribuinteBeneficio
                .Where(cb => cb.ContribuinteId == contribuinte.Id)
                .Select(cb => new SelectListItem {
                    Value = cb.BeneficioId.ToString(),
                    Text = cb.Beneficio.nome_beneficio
                }).ToList();

            ViewBag.Beneficios = beneficiosList;

            var beneficioSelecionado = _bancoContext.Beneficio
                .FirstOrDefault(b => b.Id == pagamento.BeneficioId);
            if (pagamento.BeneficioId == null) {
                pagamento.valor_total = pagamento.valor_inicial;
                pagamento.Desconto = 0;
            }
            else {
                pagamento.valor_total = pagamento.valor_inicial - (pagamento.valor_inicial * (beneficioSelecionado.percentual / 100));
                pagamento.Desconto = beneficioSelecionado.percentual;
            }

            return View(pagamento);
        }

        public IActionResult ObterBeneficios(int contribuinteId) {
            var beneficios = _bancoContext.ContribuinteBeneficio
            .Where(cb => cb.ContribuinteId == contribuinteId)
            .Select(cb => new {
                value = cb.BeneficioId.ToString(),
                text = cb.Beneficio.nome_beneficio,
                percentual = cb.Beneficio.percentual // Adicionando o percentual aqui
            }).ToList();

            return Json(beneficios);
        }

    }
}
