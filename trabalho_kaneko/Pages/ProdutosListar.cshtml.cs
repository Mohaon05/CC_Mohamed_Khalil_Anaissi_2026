using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class ProdutosListarModel : PageModel
    {
        private readonly ProdutoRepository _produtoRepository;

        public ProdutosListarModel(ProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public List<ProdutoModel> ListaProdutos { get; set; } = new List<ProdutoModel>();

        public void OnGet()
        {
            ListaProdutos = _produtoRepository.ListarTodos();
        }

        public IActionResult OnPostDeletar(int id)
        {
            bool sucesso = _produtoRepository.Excluir(id);
            if (sucesso) TempData["MensagemSucesso"] = "Produto excluído com sucesso!";
            else TempData["MensagemErro"] = "Não foi possível excluir o produto. Verifique se ele está vinculado a movimentações.";
            return RedirectToPage();
        }
    }
}