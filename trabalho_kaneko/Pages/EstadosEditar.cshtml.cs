using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class EstadosEditarModel : PageModel
    {
        private readonly EstadoRepository _estadoRepository;
        private readonly PaisRepository _paisRepository;

        public EstadosEditarModel(EstadoRepository estadoRepository, PaisRepository paisRepository)
        {
            _estadoRepository = estadoRepository;
            _paisRepository = paisRepository;
        }

        [BindProperty]
        public EstadoModel Estado { get; set; }

        // Lista para preencher o dropdown de países
        public List<PaisModel> ListaPaisesDisponiveis { get; set; } = new List<PaisModel>();

        public IActionResult OnGet(int id)
        {
            Estado = _estadoRepository.BuscarPorId(id);

            if (Estado == null)
            {
                return RedirectToPage("/EstadosListar");
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

            // NOVA TRAVA DE SEGURANÇA (Usando 'Estado')
            if (_estadoRepository.ExisteEstadoNoPais(Estado.Estado, Estado.Uf, Estado.IdPais, Estado.IdEstado))
            {
                ModelState.AddModelError(string.Empty, "Erro: Este Nome ou UF já pertence a outro Estado neste País!");
                CarregarListas();
                return Page();
            }

            bool sucesso = _estadoRepository.Atualizar(Estado);

            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Estado atualizado com sucesso!";
                return RedirectToPage("/EstadosListar");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Erro ao atualizar o estado no banco de dados.");
                CarregarListas();
                return Page();
            }
        }

        private void CarregarListas()
        {
            ListaPaisesDisponiveis = _paisRepository.ListarTodos();
        }

        // ========================================================================
        // MÉTODOS AJAX: GESTÃO RÁPIDA DE PAÍSES DENTRO DA TELA DE EDITAR ESTADO
        // ========================================================================

        public JsonResult OnPostCriarPaisRapido(string paisNome, string paisSigla, string paisDdi, string paisMoeda)
        {
            if (string.IsNullOrEmpty(paisNome) || string.IsNullOrEmpty(paisSigla))
            {
                return new JsonResult(new { sucesso = false });
            }

            var novoPais = new PaisModel
            {
                Pais = paisNome,
                Sigla = paisSigla,
                Ddi = paisDdi,
                Moeda = paisMoeda
            };

            int novoId = _paisRepository.InserirRetornandoId(novoPais);

            if (novoId > 0)
            {
                return new JsonResult(new { sucesso = true, id = novoId, nome = novoPais.Pais });
            }

            return new JsonResult(new { sucesso = false });
        }

        public JsonResult OnPostEditarPaisRapido(int id, string paisNome, string paisSigla, string paisDdi, string paisMoeda)
        {
            if (id <= 0 || string.IsNullOrEmpty(paisNome))
                return new JsonResult(new { sucesso = false, mensagem = "Dados inválidos." });

            var paisEditado = new PaisModel
            {
                IdPais = id,
                Pais = paisNome,
                Sigla = paisSigla,
                Ddi = paisDdi,
                Moeda = paisMoeda
            };

            bool sucesso = _paisRepository.Atualizar(paisEditado);

            if (sucesso) return new JsonResult(new { sucesso = true });
            return new JsonResult(new { sucesso = false, mensagem = "Erro ao atualizar país no banco." });
        }

        public JsonResult OnPostExcluirPaisRapido(int id)
        {
            if (id <= 0) return new JsonResult(new { sucesso = false });

            bool sucesso = _paisRepository.Excluir(id);

            if (sucesso) return new JsonResult(new { sucesso = true });

            return new JsonResult(new { sucesso = false, mensagem = "Não é possível excluir este país pois ele já está vinculado a outro cadastro (como a um Estado)." });
        }
    }
}