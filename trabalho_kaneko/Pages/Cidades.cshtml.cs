using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class CidadesModel : PageModel
    {
        private readonly CidadeRepository _cidadeRepository;
        private readonly EstadoRepository _estadoRepository;
        private readonly PaisRepository _paisRepository;

        public CidadesModel(CidadeRepository cidadeRepository, EstadoRepository estadoRepository, PaisRepository paisRepository)
        {
            _cidadeRepository = cidadeRepository;
            _estadoRepository = estadoRepository;
            _paisRepository = paisRepository;
        }

        [BindProperty]
        public CidadeModel Cidade { get; set; }

        public List<EstadoModel> ListaEstados { get; set; } = new List<EstadoModel>();
        public List<PaisModel> ListaPaises { get; set; } = new List<PaisModel>();

        public void OnGet()
        {
            CarregarListas();
            ListaPaises = _paisRepository.ListarTodos();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                CarregarListas();
                return Page();
            }

            bool sucesso = _cidadeRepository.Inserir(Cidade);

            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Cidade cadastrada com sucesso!";

                // MODIFICADO: Redireciona para a nova tela de listagem de cidades
                return RedirectToPage("/CidadesListar");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Erro ao salvar a cidade no banco de dados.");
                CarregarListas();
                return Page();
            }
        }

        private void CarregarListas()
        {
            // MODIFICADO: Carrega apenas a lista de estados para o formulário
            ListaEstados = _estadoRepository.ListarTodos();
        }

        public JsonResult OnPostCriarEstadoRapido(string estadoNome, string estadoUf, int idPais)
        {
            if (string.IsNullOrEmpty(estadoNome) || idPais <= 0) return new JsonResult(new { sucesso = false });

            var novoEstado = new EstadoModel { Estado = estadoNome, Uf = estadoUf, IdPais = idPais };
            int novoId = _estadoRepository.InserirRetornandoId(novoEstado); // Garanta que injetou o _estadoRepository!

            if (novoId > 0) return new JsonResult(new { sucesso = true, id = novoId, nome = $"{novoEstado.Estado} - {novoEstado.Uf}" });
            return new JsonResult(new { sucesso = false });
        }

        public JsonResult OnPostCriarPaisRapido(string paisNome, string paisSigla, string paisDdi, string paisMoeda)
        {
            if (string.IsNullOrEmpty(paisNome)) return new JsonResult(new { sucesso = false });

            var novoPais = new PaisModel { Pais = paisNome, Sigla = paisSigla, Ddi = paisDdi, Moeda = paisMoeda };
            int novoId = _paisRepository.InserirRetornandoId(novoPais); // Garanta que injetou o _paisRepository no construtor!

            if (novoId > 0) return new JsonResult(new { sucesso = true, id = novoId, nome = novoPais.Pais });
            return new JsonResult(new { sucesso = false });
        }


    }
}