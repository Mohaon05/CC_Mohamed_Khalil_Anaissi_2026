using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class FuncionariosEditarModel : PageModel
    {
        private readonly FuncionarioRepository _funcionarioRepository;
        private readonly CidadeRepository _cidadeRepository;
        private readonly CargoRepository _cargoRepository;

        public FuncionariosEditarModel(FuncionarioRepository funcionarioRepository, CidadeRepository cityRepository, CargoRepository cargoRepository)
        {
            _funcionarioRepository = funcionarioRepository;
            _cidadeRepository = cityRepository;
            _cargoRepository = cargoRepository;
        }

        [BindProperty]
        public FuncionarioModel Funcionario { get; set; }

        public List<CidadeModel> ListaCidades { get; set; } = new List<CidadeModel>();
        public List<CargoModel> ListaCargos { get; set; } = new List<CargoModel>();

        public IActionResult OnGet(int id)
        {
            Funcionario = _funcionarioRepository.BuscarPorId(id);
            if (Funcionario == null)
            {
                TempData["MensagemErro"] = "Funcionário não encontrado.";
                return RedirectToPage("/FuncionariosListar");
            }

            CarregarListas();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                CarregarListas();
                return Page();
            }

            bool sucesso = _funcionarioRepository.Modificar(Funcionario);
            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Funcionário atualizado com sucesso!";
                return RedirectToPage("/FuncionariosListar");
            }

            TempData["MensagemErro"] = "Erro ao atualizar dados do funcionário.";
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