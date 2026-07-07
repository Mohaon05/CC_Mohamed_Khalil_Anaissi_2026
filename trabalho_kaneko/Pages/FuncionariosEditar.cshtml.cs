using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class FuncionariosEditarModel : PageModel
    {
        // 1. INJEÇÃO DOS REPOSITÓRIOS
        private readonly FuncionarioRepository _funcionarioRepository;
        private readonly CargoRepository _cargoRepository;
        private readonly CidadeRepository _cidadeRepository;
        private readonly EstadoRepository _estadoRepository;
        private readonly PaisRepository _paisRepository;

        // 2. CONSTRUTOR
        public FuncionariosEditarModel(
            FuncionarioRepository funcionarioRepository,
            CargoRepository cargoRepository,
            CidadeRepository cidadeRepository,
            EstadoRepository estadoRepository,
            PaisRepository paisRepository)
        {
            _funcionarioRepository = funcionarioRepository;
            _cargoRepository = cargoRepository;
            _cidadeRepository = cidadeRepository;
            _estadoRepository = estadoRepository;
            _paisRepository = paisRepository;
        }

        [BindProperty]
        public FuncionarioModel Funcionario { get; set; }

        // 3. DECLARAÇÃO DAS LISTAS
        public List<CargoModel> ListaCargos { get; set; } = new List<CargoModel>();
        public List<CidadeModel> ListaCidades { get; set; } = new List<CidadeModel>();
        public List<EstadoModel> ListaEstados { get; set; } = new List<EstadoModel>();
        public List<PaisModel> ListaPaises { get; set; } = new List<PaisModel>();

        public IActionResult OnGet(int id)
        {
            Funcionario = _funcionarioRepository.BuscarPorId(id);
            if (Funcionario == null)
            {
                TempData["MensagemErro"] = "Funcionário não encontrado.";
                return RedirectToPage("/FuncionariosListar");
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

            bool sucesso = _funcionarioRepository.Modificar(Funcionario);
            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Funcionário atualizado com sucesso!";
                return RedirectToPage("/FuncionariosListar");
            }

            TempData["MensagemErro"] = "Erro ao atualizar dados do funcionário.";
            CarregarListas();
            return Page();
        }

        private void CarregarListas()
        {
            ListaCargos = _cargoRepository.ListarTodos();
            ListaCidades = _cidadeRepository.ListarTodos();
            ListaEstados = _estadoRepository.ListarTodos();
            ListaPaises = _paisRepository.ListarTodos();
        }

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