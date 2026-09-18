using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class CidadesEditarModel : PageModel
    {
        private readonly CidadeRepository _cidadeRepository;
        private readonly EstadoRepository _estadoRepository;
        private readonly PaisRepository _paisRepository;

        public CidadesEditarModel(CidadeRepository cidadeRepository, EstadoRepository estadoRepository, PaisRepository paisRepository)
        {
            _cidadeRepository = cidadeRepository;
            _estadoRepository = estadoRepository;
            _paisRepository = paisRepository;
        }

        [BindProperty]
        public CidadeModel Cidade { get; set; }

        public List<EstadoModel> ListaEstados { get; set; } = new List<EstadoModel>();
        public List<PaisModel> ListaPaises { get; set; } = new List<PaisModel>();

        public IActionResult OnGet(int id)
        {
            Cidade = _cidadeRepository.BuscarPorId(id);
            ListaPaises = _paisRepository.ListarTodos();

            if (Cidade == null)
            {
                return RedirectToPage("/CidadesListar");
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

            bool sucesso = _cidadeRepository.Atualizar(Cidade);

            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Cidade atualizada com sucesso!";
                return RedirectToPage("/CidadesListar");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Erro ao atualizar a cidade.");
                CarregarListas();
                return Page();
            }
        }

        private void CarregarListas()
        {
            ListaEstados = _estadoRepository.ListarTodos();
        }

        // ========================================================================
        // MÉTODOS AJAX: GESTÃO RÁPIDA DE ESTADOS
        // ========================================================================
        public JsonResult OnPostCriarEstadoRapido(string estadoNome, string estadoUf, int idPais)
        {
            if (string.IsNullOrEmpty(estadoNome) || string.IsNullOrEmpty(estadoUf) || idPais <= 0)
                return new JsonResult(new { sucesso = false, mensagem = "Dados incompletos." });

            var novoEstado = new EstadoModel { Estado = estadoNome, Uf = estadoUf, IdPais = idPais };
            int novoId = _estadoRepository.InserirRetornandoId(novoEstado);

            if (novoId > 0)
                return new JsonResult(new { sucesso = true, id = novoId, nome = $"{estadoNome.ToUpper()} - {estadoUf.ToUpper()}" });

            return new JsonResult(new { sucesso = false, mensagem = "Erro ao salvar no banco." });
        }

        public JsonResult OnPostEditarEstadoRapido(int id, string estadoNome, string estadoUf, int idPais)
        {
            if (id <= 0 || string.IsNullOrEmpty(estadoNome) || string.IsNullOrEmpty(estadoUf) || idPais <= 0)
                return new JsonResult(new { sucesso = false, mensagem = "Dados inválidos." });

            var estadoEditado = new EstadoModel { IdEstado = id, Estado = estadoNome, Uf = estadoUf, IdPais = idPais };
            bool sucesso = _estadoRepository.Atualizar(estadoEditado);

            if (sucesso) return new JsonResult(new { sucesso = true, nome = $"{estadoNome.ToUpper()} - {estadoUf.ToUpper()}" });
            return new JsonResult(new { sucesso = false, mensagem = "Erro ao atualizar estado." });
        }

        public JsonResult OnPostExcluirEstadoRapido(int id)
        {
            if (id <= 0) return new JsonResult(new { sucesso = false });

            bool sucesso = _estadoRepository.Excluir(id);
            if (sucesso) return new JsonResult(new { sucesso = true });

            return new JsonResult(new { sucesso = false, mensagem = "Não é possível excluir este estado pois ele já está vinculado a uma Cidade." });
        }

        // ========================================================================
        // MÉTODOS AJAX: GESTÃO RÁPIDA DE PAÍSES
        // ========================================================================
        public JsonResult OnPostCriarPaisRapido(string paisNome, string paisSigla, string paisDdi, string paisMoeda)
        {
            if (string.IsNullOrEmpty(paisNome) || string.IsNullOrEmpty(paisSigla))
                return new JsonResult(new { sucesso = false });

            var novoPais = new PaisModel { Pais = paisNome, Sigla = paisSigla, Ddi = paisDdi, Moeda = paisMoeda };
            int novoId = _paisRepository.InserirRetornandoId(novoPais);

            if (novoId > 0) return new JsonResult(new { sucesso = true, id = novoId, nome = novoPais.Pais });
            return new JsonResult(new { sucesso = false });
        }

        public JsonResult OnPostEditarPaisRapido(int id, string paisNome, string paisSigla, string paisDdi, string paisMoeda)
        {
            if (id <= 0 || string.IsNullOrEmpty(paisNome))
                return new JsonResult(new { sucesso = false, mensagem = "Dados inválidos." });

            var paisEditado = new PaisModel { IdPais = id, Pais = paisNome, Sigla = paisSigla, Ddi = paisDdi, Moeda = paisMoeda };
            bool sucesso = _paisRepository.Atualizar(paisEditado);

            if (sucesso) return new JsonResult(new { sucesso = true });
            return new JsonResult(new { sucesso = false, mensagem = "Erro ao atualizar país." });
        }

        public JsonResult OnPostExcluirPaisRapido(int id)
        {
            if (id <= 0) return new JsonResult(new { sucesso = false });

            bool sucesso = _paisRepository.Excluir(id);
            if (sucesso) return new JsonResult(new { sucesso = true });

            return new JsonResult(new { sucesso = false, mensagem = "Não é possível excluir este país pois ele já está vinculado a um Estado." });
        }
    }
}