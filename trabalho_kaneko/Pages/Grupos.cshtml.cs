using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class GruposModel : PageModel
    {
        private readonly GrupoRepository _grupoRepository;

        public GruposModel(GrupoRepository grupoRepository)
        {
            _grupoRepository = grupoRepository;
        }

        [BindProperty]
        public GrupoModel GrupoObj { get; set; } // Nome 'GrupoObj' para evitar conflito

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            bool sucesso = _grupoRepository.Inserir(GrupoObj);
            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Grupo cadastrado com sucesso!";
                return RedirectToPage("/GruposListar");
            }

            ModelState.AddModelError(string.Empty, "Erro ao salvar o grupo.");
            return Page();
        }
    }
}