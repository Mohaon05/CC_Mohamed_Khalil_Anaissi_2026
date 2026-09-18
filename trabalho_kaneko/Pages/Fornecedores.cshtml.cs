using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class FornecedoresModel : PageModel
    {
        // 1. INJEÇÃO DOS REPOSITÓRIOS
        private readonly FornecedorRepository _fornecedorRepository;
        private readonly CidadeRepository _cidadeRepository;
        private readonly EstadoRepository _estadoRepository;
        private readonly PaisRepository _paisRepository;

        // 2. CONSTRUTOR
        public FornecedoresModel(
            FornecedorRepository fornecedorRepository,
            CidadeRepository cidadeRepository,
            EstadoRepository estadoRepository,
            PaisRepository paisRepository)
        {
            _fornecedorRepository = fornecedorRepository;
            _cidadeRepository = cidadeRepository;
            _estadoRepository = estadoRepository;
            _paisRepository = paisRepository;
        }

        [BindProperty]
        public FornecedorModel Fornecedor { get; set; }

        // 3. DECLARAÇÃO DAS LISTAS
        public List<CidadeModel> ListaCidades { get; set; } = new List<CidadeModel>();
        public List<EstadoModel> ListaEstados { get; set; } = new List<EstadoModel>();
        public List<PaisModel> ListaPaises { get; set; } = new List<PaisModel>();

        public void OnGet()
        {
            CarregarListas();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                CarregarListas();
                return Page();
            }

            bool sucesso = _fornecedorRepository.Inserir(Fornecedor);
            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Fornecedor cadastrado com sucesso!";
                return RedirectToPage("/FornecedoresListar");
            }

            ModelState.AddModelError(string.Empty, "Erro ao salvar o Fornecedor. Verifique se o CPF já não existe.");
            CarregarListas();
            return Page();
        }

        private void CarregarListas()
        {
            ListaCidades = _cidadeRepository.ListarTodos();
            ListaEstados = _estadoRepository.ListarTodos();
            ListaPaises = _paisRepository.ListarTodos();
        }

        public JsonResult OnPostCriarCidadeRapido(string cidadeNome, int idEstado)
        {
            if (string.IsNullOrEmpty(cidadeNome) || idEstado <= 0) return new JsonResult(new { sucesso = false });
            var novaCidade = new CidadeModel { Cidade = cidadeNome, IdEstado = idEstado };
            int novoId = _cidadeRepository.InserirRetornandoId(novaCidade);
            if (novoId > 0) return new JsonResult(new { sucesso = true, id = novoId, nome = novaCidade.Cidade });
            return new JsonResult(new { sucesso = false });
        }

        public JsonResult OnPostCriarEstadoRapido(string estadoNome, string estadoUf, int idPais)
        {
            if (string.IsNullOrEmpty(estadoNome) || idPais <= 0) return new JsonResult(new { sucesso = false });
            var novoEstado = new EstadoModel { Estado = estadoNome, Uf = estadoUf, IdPais = idPais };
            int novoId = _estadoRepository.InserirRetornandoId(novoEstado);
            if (novoId > 0) return new JsonResult(new { sucesso = true, id = novoId, nome = novoEstado.Estado });
            return new JsonResult(new { sucesso = false });
        }

        public JsonResult OnPostCriarPaisRapido(string paisNome, string paisSigla, string paisDdi, string paisMoeda)
        {
            if (string.IsNullOrEmpty(paisNome)) return new JsonResult(new { sucesso = false });
            var novoPais = new PaisModel { Pais = paisNome, Sigla = paisSigla, Ddi = paisDdi, Moeda = paisMoeda };
            int novoId = _paisRepository.InserirRetornandoId(novoPais);
            if (novoId > 0) return new JsonResult(new { sucesso = true, id = novoId, nome = novoPais.Pais });
            return new JsonResult(new { sucesso = false });

        }
    }
}