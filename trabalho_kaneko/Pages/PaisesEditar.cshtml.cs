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
        public PaisModel Pais { get; set; }

        public IActionResult OnGet(int id)
        {
            // Quando a tela abre, busca os dados do país pelo ID
            Pais = _paisRepository.BuscarPorId(id);

            // Se alguém digitar um ID falso na URL, manda de volta pra lista
            if (Pais == null)
            {
                return RedirectToPage("/PaisesListar");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Chama o método de UPDATE
            bool sucesso = _paisRepository.Atualizar(Pais);

            if (sucesso)
            {
                TempData["MensagemSucesso"] = "País atualizado com sucesso!";
                return RedirectToPage("/PaisesListar");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Erro ao atualizar o país no banco de dados.");
                return Page();
            }
        }
    }
}