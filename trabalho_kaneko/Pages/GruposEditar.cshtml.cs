using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class GruposEditarModel : PageModel
    {
        private readonly GrupoRepository _grupoRepository;

        public GruposEditarModel(GrupoRepository grupoRepository)
        {
            _grupoRepository = grupoRepository;
        }

        [BindProperty]
        public GrupoModel GrupoObj { get; set; }

        public IActionResult OnGet(int id)
        {
            GrupoObj = _grupoRepository.BuscarPorId(id);
            if (GrupoObj == null)
            {
                return RedirectToPage("/GruposListar");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            bool sucesso = _grupoRepository.Atualizar(GrupoObj);
            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Grupo atualizado com sucesso!";
                return RedirectToPage("/GruposListar");
            }

            ModelState.AddModelError(string.Empty, "Erro ao atualizar.");
            return Page();
        }
    }
}