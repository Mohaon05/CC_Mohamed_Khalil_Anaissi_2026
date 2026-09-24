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

        public ProdutosEditarModel(
            ProdutoRepository produtoRepository,
            MarcaRepository marcaRepository,
            GrupoRepository grupoRepository)
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
                TempData["MensagemErro"] = "Produto não encontrado.";
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

            ModelState.AddModelError(string.Empty, "Erro ao atualizar dados do produto.");
            CarregarListas();
            return Page();
        }

        private void CarregarListas()
        {
            ListaMarcas = _marcaRepository.ListarTodos();
            ListaGrupos = _grupoRepository.ListarTodos();
        }

        // ========================================================================
        // MÉTODOS AJAX: GESTÃO RÁPIDA DE GRUPOS
        // ========================================================================
        public JsonResult OnPostCriarGrupoRapido(string grupoNome)
        {
            if (string.IsNullOrEmpty(grupoNome))
                return new JsonResult(new { sucesso = false, mensagem = "Dados incompletos." });

            var novoGrupo = new GrupoModel { Grupo = grupoNome };
            int novoId = _grupoRepository.InserirRetornandoId(novoGrupo);

            if (novoId > 0) return new JsonResult(new { sucesso = true, id = novoId, nome = novoGrupo.Grupo.ToUpper() });
            return new JsonResult(new { sucesso = false, mensagem = "Erro ao salvar grupo no banco." });
        }

        public JsonResult OnPostEditarGrupoRapido(int id, string grupoNome)
        {
            if (id <= 0 || string.IsNullOrEmpty(grupoNome))
                return new JsonResult(new { sucesso = false, mensagem = "Dados inválidos." });

            var grupoEditado = new GrupoModel { IdGrupo = id, Grupo = grupoNome };
            bool sucesso = _grupoRepository.Atualizar(grupoEditado);

            if (sucesso) return new JsonResult(new { sucesso = true });
            return new JsonResult(new { sucesso = false, mensagem = "Erro ao atualizar grupo." });
        }

        public JsonResult OnPostExcluirGrupoRapido(int id)
        {
            if (id <= 0) return new JsonResult(new { sucesso = false });

            bool sucesso = _grupoRepository.Excluir(id);
            if (sucesso) return new JsonResult(new { sucesso = true });

            return new JsonResult(new { sucesso = false, mensagem = "Não é possível excluir este grupo pois ele já está vinculado a um Produto." });
        }

        // ========================================================================
        // MÉTODOS AJAX: GESTÃO RÁPIDA DE MARCAS
        // ========================================================================
        public JsonResult OnPostCriarMarcaRapido(string marcaNome)
        {
            if (string.IsNullOrEmpty(marcaNome))
                return new JsonResult(new { sucesso = false, mensagem = "Dados incompletos." });

            var novaMarca = new MarcaModel { Marca = marcaNome };
            int novoId = _marcaRepository.InserirRetornandoId(novaMarca);

            if (novoId > 0) return new JsonResult(new { sucesso = true, id = novoId, nome = novaMarca.Marca.ToUpper() });
            return new JsonResult(new { sucesso = false, mensagem = "Erro ao salvar marca no banco." });
        }

        public JsonResult OnPostEditarMarcaRapido(int id, string marcaNome)
        {
            if (id <= 0 || string.IsNullOrEmpty(marcaNome))
                return new JsonResult(new { sucesso = false, mensagem = "Dados inválidos." });

            var marcaEditada = new MarcaModel { IdMarca = id, Marca = marcaNome };
            bool sucesso = _marcaRepository.Atualizar(marcaEditada);

            if (sucesso) return new JsonResult(new { sucesso = true });
            return new JsonResult(new { sucesso = false, mensagem = "Erro ao atualizar marca." });
        }

        public JsonResult OnPostExcluirMarcaRapido(int id)
        {
            if (id <= 0) return new JsonResult(new { sucesso = false });

            bool sucesso = _marcaRepository.Excluir(id);
            if (sucesso) return new JsonResult(new { sucesso = true });

            return new JsonResult(new { sucesso = false, mensagem = "Não é possível excluir esta marca pois ela já está vinculada a um Produto." });
        }
    }
}