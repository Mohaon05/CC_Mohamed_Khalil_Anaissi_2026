using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class CondicoesPagamentoModel : PageModel
    {
        private readonly CondicaoPagamentoRepository _condicaoRepository;

        public CondicoesPagamentoModel(CondicaoPagamentoRepository condicaoRepository)
        {
            _condicaoRepository = condicaoRepository;
        }

        [BindProperty]
        public CondicaoPagamentoModel CondicaoObj { get; set; }

        public void OnGet()
        {
            // Espelhando o banco: Qtd = 1 e Juros = 0.00
            CondicaoObj = new CondicaoPagamentoModel
            {
                QuantidadeParcelas = 1,
                Juros = 0.00m
            };
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            bool sucesso = _condicaoRepository.Inserir(CondicaoObj);
            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Condição cadastrada com sucesso!";
                return RedirectToPage("/CondicoesPagamentoListar");
            }

            ModelState.AddModelError(string.Empty, "Erro ao salvar os dados.");
            return Page();
        }
    }
}