using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using trabajo_kaneko.Repository;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class FormasPagamentoEditarModel : PageModel
    {
        private readonly FormaPagamentoRepository _formaRepository;

        public FormasPagamentoEditarModel(FormaPagamentoRepository formaRepository)
        {
            _formaRepository = formaRepository;
        }

        [BindProperty]
        public FormaPagamentoModel FormaObj { get; set; }

        public IActionResult OnGet(int id)
        {
            FormaObj = _formaRepository.BuscarPorId(id);
            if (FormaObj == null)
            {
                return RedirectToPage("/FormasPagamentoListar");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            bool sucesso = _formaRepository.Atualizar(FormaObj);
            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Forma de pagamento atualizada com sucesso!";
                return RedirectToPage("/FormasPagamentoListar");
            }

            ModelState.AddModelError(string.Empty, "Erro ao atualizar.");
            return Page();
        }
    }
}