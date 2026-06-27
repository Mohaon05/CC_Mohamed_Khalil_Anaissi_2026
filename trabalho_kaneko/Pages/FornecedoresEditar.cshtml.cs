using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class FornecedoresEditarModel : PageModel
    {
        private readonly FornecedorRepository _fornecedorRepository;
        private readonly CidadeRepository _cidadeRepository;

        public FornecedoresEditarModel(FornecedorRepository fornecedorRepository, CidadeRepository cityRepository)
        {
            _fornecedorRepository = fornecedorRepository;
            _cidadeRepository = cityRepository;
        }

        [BindProperty]
        public FornecedorModel Fornecedor { get; set; }

        public List<CidadeModel> ListaCidades { get; set; } = new List<CidadeModel>();

        public IActionResult OnGet(int id)
        {
            // Carrega o fornecedor específico pelo ID e a lista de cidades
            Fornecedor = _fornecedorRepository.BuscarPorId(id);
            ListaCidades = _cidadeRepository.ListarTodos();

            if (Fornecedor == null)
            {
                TempData["MensagemErro"] = "Fornecedor não encontrado.";
                return RedirectToPage("/FornecedoresListar");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ListaCidades = _cidadeRepository.ListarTodos();
                return Page();
            }

            bool sucesso = _fornecedorRepository.Atualizar(Fornecedor);

            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Fornecedor atualizado com sucesso!";
                return RedirectToPage("/FornecedoresListar");
            }
            else
            {
                TempData["MensagemErro"] = "Erro ao atualizar o fornecedor no banco de dados.";
                ListaCidades = _cidadeRepository.ListarTodos();
                return Page();
            }
        }
    }
}