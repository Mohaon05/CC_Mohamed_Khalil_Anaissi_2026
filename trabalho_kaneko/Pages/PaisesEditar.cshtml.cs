using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class PaisesEditarModel : PageModel
    {
        private readonly PaisRepository _paisRepository;

        public PaisesEditarModel(PaisRepository paisRepository)
        {
            _paisRepository = paisRepository;
        }

        [BindProperty]
        public PaisModel PaisObj { get; set; }

        public IActionResult OnGet(int id)
        {
            PaisObj = _paisRepository.BuscarPorId(id);
            if (PaisObj == null)
            {
                return RedirectToPage("/PaisesListar");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            bool sucesso = _paisRepository.Atualizar(PaisObj);
            if (sucesso)
            {
                TempData["MensagemSucesso"] = "País atualizado com sucesso!";
                return RedirectToPage("/PaisesListar");
            }

            ModelState.AddModelError(string.Empty, "Erro ao atualizar.");
            return Page();
        }
    }
}