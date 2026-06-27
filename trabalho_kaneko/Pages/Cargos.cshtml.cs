using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class CargosModel : PageModel
    {
        private readonly CargoRepository _cargoRepository;

        public CargosModel(CargoRepository cargoRepository)
        {
            _cargoRepository = cargoRepository;
        }

        [BindProperty]
        public CargoModel CargoObj { get; set; } // Nome diferente para evitar conflito com a string 'Cargo'

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            bool sucesso = _cargoRepository.Inserir(CargoObj);
            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Cargo cadastrado com sucesso!";
                return RedirectToPage("/CargosListar");
            }

            ModelState.AddModelError(string.Empty, "Erro ao salvar o cargo.");
            return Page();
        }
    }
}