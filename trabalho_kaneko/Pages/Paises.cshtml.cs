using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class PaisesModel : PageModel
    {
        private readonly PaisRepository _paisRepository;

        public PaisesModel(PaisRepository paisRepository)
        {
            _paisRepository = paisRepository;
        }

        [BindProperty]
        public PaisModel PaisObj { get; set; }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            bool sucesso = _paisRepository.Inserir(PaisObj);
            if (sucesso)
            {
                TempData["MensagemSucesso"] = "País cadastrado com sucesso!";
                return RedirectToPage("/PaisesListar");
            }

            ModelState.AddModelError(string.Empty, "Erro ao salvar o país.");
            return Page();
        }
    }
}