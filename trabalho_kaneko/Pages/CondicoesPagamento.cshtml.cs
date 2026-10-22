using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository; // Necessário para aceder ao repositório

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
        public CondicaoPagamentoModel Condicao { get; set; }

        public void OnGet()
        {
            // Ao abrir a página, define os valores padrão
            if (Condicao == null)
            {
                Condicao = new CondicaoPagamentoModel
                {
                    QuantidadeParcelas = 1,
                    Juros = 0.00m,
                    Multa = 0.00m,
                    Desconto = 0.00m
                };
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool sucesso = _condicaoRepository.Inserir(Condicao);

            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Condição de pagamento guardada com sucesso!";
                return RedirectToPage("/CondicoesPagamentoListar");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Erro ao guardar a condição de pagamento na base de dados.");
                return Page();
            }
        }
    }
}