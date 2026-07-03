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

        // Método para salvar País direto da tela de Editar Estado
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
    }
}