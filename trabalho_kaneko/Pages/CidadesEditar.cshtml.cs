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

        public JsonResult OnPostCriarPaisRapido(string paisNome, string paisSigla, string paisDdi, string paisMoeda)
        {
            if (string.IsNullOrEmpty(paisNome)) return new JsonResult(new { sucesso = false });

            // Ajuste "PaisModel" para o nome exato da sua classe de modelo, caso seja diferente
            var novoPais = new PaisModel { Pais = paisNome, Sigla = paisSigla, Ddi = paisDdi, Moeda = paisMoeda };
            int novoId = _paisRepository.InserirRetornandoId(novoPais);

            if (novoId > 0) return new JsonResult(new { sucesso = true, id = novoId, nome = novoPais.Pais });
            return new JsonResult(new { sucesso = false });
        }

        public JsonResult OnPostCriarEstadoRapido(string estadoNome, string estadoUf, int idPais)
        {
            if (string.IsNullOrEmpty(estadoNome) || idPais <= 0) return new JsonResult(new { sucesso = false });

            // Ajuste "EstadoModel" para o nome exato da sua classe de modelo
            var novoEstado = new EstadoModel { Estado = estadoNome, Uf = estadoUf, IdPais = idPais };
            int novoId = _estadoRepository.InserirRetornandoId(novoEstado);

            if (novoId > 0) return new JsonResult(new { sucesso = true, id = novoId, nome = $"{novoEstado.Estado} - {novoEstado.Uf}" });
            return new JsonResult(new { sucesso = false });
        }
    }
}