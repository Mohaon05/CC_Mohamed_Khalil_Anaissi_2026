using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class CondicoesPagamentoEditarModel : PageModel
    {
        private readonly CondicaoPagamentoRepository _condicaoRepository;

        public CondicoesPagamentoEditarModel(CondicaoPagamentoRepository condicaoRepository)
        {
            _condicaoRepository = condicaoRepository;
        }

        [BindProperty]
        public CondicaoPagamentoModel CondicaoObj { get; set; }

        public IActionResult OnGet(int id)
        {
            CondicaoObj = _condicaoRepository.BuscarPorId(id);
            if (CondicaoObj == null)
            {
                return RedirectToPage("/CondicoesPagamentoListar");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            bool sucesso = _condicaoRepository.Atualizar(CondicaoObj);
            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Condição atualizada com sucesso!";
                return RedirectToPage("/CondicoesPagamentoListar");
            }

            ModelState.AddModelError(string.Empty, "Erro ao atualizar.");
            return Page();
        }
    }
}