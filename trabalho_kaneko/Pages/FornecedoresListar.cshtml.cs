using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class FornecedoresListarModel : PageModel
    {
        private readonly FornecedorRepository _fornecedorRepository;

        public FornecedoresListarModel(FornecedorRepository fornecedorRepository)
        {
            _fornecedorRepository = fornecedorRepository;
        }

        public List<FornecedorModel> ListaFornecedores { get; set; } = new List<FornecedorModel>();

        public void OnGet()
        {
            ListaFornecedores = _fornecedorRepository.ListarTodos();
        }

        public IActionResult OnPostDeletar(int id)
        {
            bool sucesso = _fornecedorRepository.Excluir(id);

            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Fornecedor excluído com sucesso!";
            }
            else
            {
                TempData["MensagemErro"] = "Não foi possível excluir o fornecedor.";
            }

            return RedirectToPage();
        }
    }
}