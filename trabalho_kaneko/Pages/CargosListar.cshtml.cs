using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class CargosListarModel : PageModel
    {
        private readonly CargoRepository _cargoRepository;

        public CargosListarModel(CargoRepository cargoRepository)
        {
            _cargoRepository = cargoRepository;
        }

        public List<CargoModel> ListaCargos { get; set; } = new List<CargoModel>();

        public void OnGet()
        {
            ListaCargos = _cargoRepository.ListarTodos();
        }

        public IActionResult OnPostDeletar(int id)
        {
            bool sucesso = _cargoRepository.Excluir(id);
            if (sucesso) TempData["MensagemSucesso"] = "Cargo excluído com sucesso!";
            else TempData["MensagemErro"] = "Não foi possível excluir o cargo. Verifique se ele está em uso.";
            return RedirectToPage();
        }
    }
}