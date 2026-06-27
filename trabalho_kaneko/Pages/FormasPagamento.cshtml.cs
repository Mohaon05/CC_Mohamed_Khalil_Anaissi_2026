using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using trabajo_kaneko.Repository;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class FormasPagamentoModel : PageModel
    {
        private readonly FormaPagamentoRepository _formaRepository;

        public FormasPagamentoModel(FormaPagamentoRepository formaRepository)
        {
            _formaRepository = formaRepository;
        }

        [BindProperty]
        public FormaPagamentoModel FormaObj { get; set; }

        public void OnGet()
        {
            // Força o checkbox a vir marcado por padrão ao criar um novo registro
            FormaObj = new FormaPagamentoModel { Ativo = true };
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            bool sucesso = _formaRepository.Inserir(FormaObj);
            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Forma de pagamento cadastrada com sucesso!";
                return RedirectToPage("/FormasPagamentoListar");
            }

            ModelState.AddModelError(string.Empty, "Erro ao salvar os dados.");
            return Page();
        }
    }
}