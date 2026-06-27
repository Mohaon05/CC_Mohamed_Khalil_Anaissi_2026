using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class FuncionariosModel : PageModel
    {
        private readonly FuncionarioRepository _funcionarioRepository;
        private readonly CidadeRepository _cidadeRepository;
        private readonly CargoRepository _cargoRepository;

        public FuncionariosModel(FuncionarioRepository funcionarioRepository, CidadeRepository cityRepository, CargoRepository cargoRepository)
        {
            _funcionarioRepository = funcionarioRepository;
            _cidadeRepository = cityRepository;
            _cargoRepository = cargoRepository;
        }

        [BindProperty]
        public FuncionarioModel Funcionario { get; set; }

        public List<CidadeModel> ListaCidades { get; set; } = new List<CidadeModel>();
        public List<CargoModel> ListaCargos { get; set; } = new List<CargoModel>();

        public void OnGet()
        {
            CarregarListas();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                CarregarListas();
                return Page();
            }

            bool sucesso = _funcionarioRepository.Inserir(Funcionario);
            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Funcionário cadastrado com sucesso!";
                return RedirectToPage("/FuncionariosListar");
            }

            ModelState.AddModelError(string.Empty, "Erro ao salvar o funcionário. Verifique se o CPF já não existe.");
            CarregarListas();
            return Page();
        }

        private void CarregarListas()
        {
            ListaCidades = _cidadeRepository.ListarTodos();
            ListaCargos = _cargoRepository.ListarTodos();
        }
    }
}