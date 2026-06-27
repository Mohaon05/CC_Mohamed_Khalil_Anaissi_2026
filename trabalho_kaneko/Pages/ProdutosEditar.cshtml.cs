using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class ProdutosEditarModel : PageModel
    {
        private readonly ProdutoRepository _produtoRepository;
        private readonly MarcaRepository _marcaRepository;
        private readonly GrupoRepository _grupoRepository;

        public ProdutosEditarModel(ProdutoRepository produtoRepository, MarcaRepository marcaRepository, GrupoRepository grupoRepository)
        {
            _produtoRepository = produtoRepository;
            _marcaRepository = marcaRepository;
            _grupoRepository = grupoRepository;
        }

        [BindProperty]
        public ProdutoModel ProdutoObj { get; set; }

        public List<MarcaModel> ListaMarcas { get; set; } = new List<MarcaModel>();
        public List<GrupoModel> ListaGrupos { get; set; } = new List<GrupoModel>();

        public IActionResult OnGet(int id)
        {
            ProdutoObj = _produtoRepository.BuscarPorId(id);
            if (ProdutoObj == null)
            {
                return RedirectToPage("/ProdutosListar");
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

            bool sucesso = _produtoRepository.Atualizar(ProdutoObj);
            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Produto atualizado com sucesso!";
                return RedirectToPage("/ProdutosListar");
            }

            ModelState.AddModelError(string.Empty, "Erro ao atualizar.");
            CarregarListas();
            return Page();
        }

        private void CarregarListas()
        {
            ListaMarcas = _marcaRepository.ListarTodos();
            ListaGrupos = _grupoRepository.ListarTodos();
        }
    }
}