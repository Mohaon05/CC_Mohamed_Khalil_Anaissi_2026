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
            CarregarListas();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                CarregarListas();
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
                CarregarListas();
                return Page();
            }
        }

        private void CarregarListas()
        {
            // Carrega apenas o necessário para popular o formulário principal e os modais aninhados
            ListaCidades = _cidadeRepository.ListarTodos();
            ListaEstados = _estadoRepository.ListarTodos();
            ListaPaises = _paisRepository.ListarTodos();
        }

        // ========================================================================
        // MÉTODOS AJAX: GESTÃO RÁPIDA DE CIDADES
        // ========================================================================
        public JsonResult OnPostCriarCidadeRapido(string cidadeNome, int idEstado)
        {
            if (string.IsNullOrEmpty(cidadeNome) || idEstado <= 0)
                return new JsonResult(new { sucesso = false, mensagem = "Dados incompletos." });

            var novaCidade = new CidadeModel { Cidade = cidadeNome, IdEstado = idEstado };
            int novoId = _cidadeRepository.InserirRetornandoId(novaCidade);

            if (novoId > 0)
                return new JsonResult(new { sucesso = true, id = novoId, nome = cidadeNome.ToUpper() });

            return new JsonResult(new { sucesso = false, mensagem = "Erro ao salvar cidade no banco." });
        }

        public JsonResult OnPostEditarCidadeRapido(int id, string cidadeNome, int idEstado)
        {
            if (id <= 0 || string.IsNullOrEmpty(cidadeNome) || idEstado <= 0)
                return new JsonResult(new { sucesso = false, mensagem = "Dados inválidos." });

            var cidadeEditada = new CidadeModel { IdCidade = id, Cidade = cidadeNome, IdEstado = idEstado };
            bool sucesso = _cidadeRepository.Atualizar(cidadeEditada);

            if (sucesso) return new JsonResult(new { sucesso = true, nome = cidadeNome.ToUpper() });
            return new JsonResult(new { sucesso = false, mensagem = "Erro ao atualizar cidade." });
        }

        public JsonResult OnPostExcluirCidadeRapido(int id)
        {
            if (id <= 0) return new JsonResult(new { sucesso = false });

            bool sucesso = _cidadeRepository.Excluir(id);
            if (sucesso) return new JsonResult(new { sucesso = true });

            return new JsonResult(new { sucesso = false, mensagem = "Não é possível excluir esta cidade pois ela já está vinculada a um Cliente/Fornecedor." });
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

            return new JsonResult(new { sucesso = false, mensagem = "Erro ao salvar estado no banco." });
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
                return new JsonResult(new { sucesso = false, mensagem = "Dados incompletos." });

            var novoPais = new PaisModel { Pais = paisNome, Sigla = paisSigla, Ddi = paisDdi, Moeda = paisMoeda };
            int novoId = _paisRepository.InserirRetornandoId(novoPais);

            if (novoId > 0) return new JsonResult(new { sucesso = true, id = novoId, nome = novoPais.Pais.ToUpper() });
            return new JsonResult(new { sucesso = false, mensagem = "Erro ao salvar país no banco." });
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