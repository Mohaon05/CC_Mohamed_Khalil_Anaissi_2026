using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class FornecedoresModel : PageModel
    {
        private readonly FornecedorRepository _fornecedorRepository;
        private readonly CidadeRepository _cidadeRepository;

        public FornecedoresModel(FornecedorRepository fornecedorRepository, CidadeRepository cidadeRepository)
        {
            _fornecedorRepository = fornecedorRepository;
            _cidadeRepository = cidadeRepository;
        }

        [BindProperty]
        public FornecedorModel Fornecedor { get; set; }

        public List<CidadeModel> ListaCidades { get; set; } = new List<CidadeModel>();

        public void OnGet()
        {
            ListaCidades = _cidadeRepository.ListarTodos();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ListaCidades = _cidadeRepository.ListarTodos();
                return Page();
            }

            bool sucesso = _fornecedorRepository.Inserir(Fornecedor);

            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Fornecedor cadastrado com sucesso!";
                return RedirectToPage("/FornecedoresListar");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Erro ao salvar. Verifique se o CPF/CNPJ já não está cadastrado.");
                ListaCidades = _cidadeRepository.ListarTodos();
                return Page();
            }
        }
    }
}
