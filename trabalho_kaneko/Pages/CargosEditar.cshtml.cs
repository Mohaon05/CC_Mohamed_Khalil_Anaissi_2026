using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class CargosEditarModel : PageModel
    {
        private readonly CargoRepository _cargoRepository;

        public CargosEditarModel(CargoRepository cargoRepository)
        {
            _cargoRepository = cargoRepository;
        }

        [BindProperty]
        public CargoModel CargoObj { get; set; }

        public IActionResult OnGet(int id)
        {
            CargoObj = _cargoRepository.BuscarPorId(id);
            if (CargoObj == null)
            {
                return RedirectToPage("/CargosListar");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            bool sucesso = _cargoRepository.Atualizar(CargoObj);
            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Cargo atualizado com sucesso!";
                return RedirectToPage("/CargosListar");
            }

            ModelState.AddModelError(string.Empty, "Erro ao atualizar.");
            return Page();
        }
    }
}