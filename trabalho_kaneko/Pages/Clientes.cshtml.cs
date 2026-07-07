using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class ClientesModel : PageModel
    {
        private readonly ClienteRepository _clienteRepository;
        private readonly CidadeRepository _cidadeRepository;
        private readonly EstadoRepository _estadoRepository;
        private readonly PaisRepository _paisRepository;

        // INJEÇÃO DOS 4 REPOSITÓRIOS
        public ClientesModel(
            ClienteRepository clienteRepository,
            CidadeRepository cidadeRepository,
            EstadoRepository estadoRepository,
            PaisRepository paisRepository)
        {
            _clienteRepository = clienteRepository;
            _cidadeRepository = cidadeRepository;
            _estadoRepository = estadoRepository;
            _paisRepository = paisRepository;
        }

        [BindProperty]
        public ClienteModel Cliente { get; set; }

        public List<CidadeModel> ListaCidades { get; set; } = new List<CidadeModel>();
        public List<EstadoModel> ListaEstados { get; set; } = new List<EstadoModel>();
        public List<PaisModel> ListaPaises { get; set; } = new List<PaisModel>();

        public void OnGet()
        {
            // Carrega tudo ao abrir a tela
            ListaCidades = _cidadeRepository.ListarTodos();
            ListaEstados = _estadoRepository.ListarTodos();
            ListaPaises = _paisRepository.ListarTodos();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ListaCidades = _cidadeRepository.ListarTodos();
                return Page();
            }

            bool sucesso = _clienteRepository.Inserir(Cliente);

            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Cliente cadastrado com sucesso!";
                return RedirectToPage("/ClientesListar");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Erro ao salvar o cliente. Verifique se o CPF/CNPJ já não está cadastrado.");
                ListaCidades = _cidadeRepository.ListarTodos();
                return Page();
            }
        }

        // --- MÉTODOS DE CADASTRO RÁPIDO VIA AJAX ---

        public JsonResult OnPostCriarPaisRapido(string paisNome, string paisSigla, string paisDdi, string paisMoeda)
        {
            if (string.IsNullOrEmpty(paisNome)) return new JsonResult(new { sucesso = false });
            var novoPais = new PaisModel { Pais = paisNome, Sigla = paisSigla, Ddi = paisDdi, Moeda = paisMoeda };
            int novoId = _paisRepository.InserirRetornandoId(novoPais);
            if (novoId > 0) return new JsonResult(new { sucesso = true, id = novoId, nome = novoPais.Pais });
            return new JsonResult(new { sucesso = false });
        }

        public JsonResult OnPostCriarEstadoRapido(string estadoNome, string estadoUf, int idPais)
        {
            if (string.IsNullOrEmpty(estadoNome) || idPais <= 0) return new JsonResult(new { sucesso = false });
            var novoEstado = new EstadoModel { Estado = estadoNome, Uf = estadoUf, IdPais = idPais };
            int novoId = _estadoRepository.InserirRetornandoId(novoEstado);
            // Caso tenha ajustado para NomePais, mude aqui também se necessário
            if (novoId > 0) return new JsonResult(new { sucesso = true, id = novoId, nome = $"{novoEstado.Estado} - {novoEstado.Uf}" });
            return new JsonResult(new { sucesso = false });
        }

        public JsonResult OnPostCriarCidadeRapido(string cidadeNome, int idEstado)
        {
            if (string.IsNullOrEmpty(cidadeNome) || idEstado <= 0) return new JsonResult(new { sucesso = false });
            var novaCidade = new CidadeModel { Cidade = cidadeNome, IdEstado = idEstado };
            int novoId = _cidadeRepository.InserirRetornandoId(novaCidade);
            if (novoId > 0) return new JsonResult(new { sucesso = true, id = novoId, nome = novaCidade.Cidade });
            return new JsonResult(new { sucesso = false });
        }
    }
}