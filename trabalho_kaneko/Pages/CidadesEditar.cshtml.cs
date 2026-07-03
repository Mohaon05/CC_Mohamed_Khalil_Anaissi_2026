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

        public List<EstadoModel> ListaEstadosDisponiveis { get; set; } = new List<EstadoModel>();
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
            ListaEstadosDisponiveis = _estadoRepository.ListarTodos();
        }

        public JsonResult OnPostCriarEstadoRapido(string estadoNome, string estadoUf, int idPais)
        {
            if (string.IsNullOrEmpty(estadoNome) || string.IsNullOrEmpty(estadoUf) || idPais <= 0)
            {
                return new JsonResult(new { sucesso = false });
            }

            var novoEstado = new EstadoModel
            {
                Estado = estadoNome,
                Uf = estadoUf,
                IdPais = idPais
            };

            int novoId = _estadoRepository.InserirRetornandoId(novoEstado);

            if (novoId > 0)
            {
                string nomeExibicao = $"{novoEstado.Estado} - {novoEstado.Uf}";
                return new JsonResult(new { sucesso = true, id = novoId, nome = nomeExibicao });
            }

            return new JsonResult(new { sucesso = false });
        }

        // Método para salvar País direto da tela de Editar Cidades
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