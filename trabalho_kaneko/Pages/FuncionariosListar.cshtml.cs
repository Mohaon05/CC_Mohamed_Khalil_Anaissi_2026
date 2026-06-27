using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class FuncionariosListarModel : PageModel
    {
        private readonly FuncionarioRepository _funcionarioRepository;

        public FuncionariosListarModel(FuncionarioRepository funcionarioRepository)
        {
            _funcionarioRepository = funcionarioRepository;
        }

        public List<FuncionarioModel> ListaFuncionarios { get; set; } = new List<FuncionarioModel>();

        public void OnGet()
        {
            ListaFuncionarios = _funcionarioRepository.ListarTodos();
        }

        public IActionResult OnPostDeletar(int id)
        {
            bool sucesso = _funcionarioRepository.Excluir(id);
            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Funcionário excluído com sucesso!";
            }
            else
            {
                TempData["MensagemErro"] = "Erro ao tentar excluir o funcionário.";
            }
            return RedirectToPage();
        }
    }
}