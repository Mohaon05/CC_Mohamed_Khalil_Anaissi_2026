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

        public FornecedoresEditarModel(FornecedorRepository fornecedorRepository, CidadeRepository cidadeRepository)
        {
            _fornecedorRepository = fornecedorRepository;
            _cidadeRepository = cidadeRepository;
        }

        [BindProperty]
        public FornecedorModel FornecedorObj { get; set; }

        public List<CidadeModel> ListaCidades { get; set; } = new List<CidadeModel>();

        public IActionResult OnGet(int id)
        {
            FornecedorObj = _fornecedorRepository.BuscarPorId(id);
            if (FornecedorObj == null)
            {
                return RedirectToPage("/FornecedoresListar");
            }
            ListaCidades = _cidadeRepository.ListarTodos();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ListaCidades = _cidadeRepository.ListarTodos();
                return Page();
            }

            bool sucesso = _fornecedorRepository.Atualizar(FornecedorObj);
            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Fornecedor atualizado com sucesso!";
                return RedirectToPage("/FornecedoresListar");
            }

            ModelState.AddModelError(string.Empty, "Erro ao atualizar dados.");
            ListaCidades = _cidadeRepository.ListarTodos();
            return Page();
        }
    }
}