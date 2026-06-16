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
        public PaisModel Pais { get; set; }

        public void OnGet()
        {
            // MODIFICADO: Removemos a busca da lista daqui. A tela carrega vazia.
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool sucesso = _paisRepository.Inserir(Pais);

            if (sucesso)
            {
                TempData["MensagemSucesso"] = "País cadastrado com sucesso!";

                // MODIFICADO: Após salvar, redirecionamos o usuário de volta para a tela de listagem
                return RedirectToPage("/PaisesListar");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Erro ao salvar o país no banco de dados.");
                return Page();
            }
        }
    }
}