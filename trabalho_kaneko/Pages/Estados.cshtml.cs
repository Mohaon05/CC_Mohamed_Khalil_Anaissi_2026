using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class EstadosModel : PageModel
    {
        private readonly EstadoRepository _estadoRepository;
        private readonly PaisRepository _paisRepository;

        public EstadosModel(EstadoRepository estadoRepository, PaisRepository paisRepository)
        {
            _estadoRepository = estadoRepository;
            _paisRepository = paisRepository;
        }

        [BindProperty]
        public EstadoModel Estado { get; set; }

        public List<PaisModel> ListaPaisesDisponiveis { get; set; } = new List<PaisModel>();

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

            // 🛑 NOVA TRAVA DE SEGURANÇA AQUI (Usando 'Estado' em vez de 'EstadoObj')
            if (_estadoRepository.ExisteEstadoNoPais(Estado.Estado, Estado.Uf, Estado.IdPais))
            {
                ModelState.AddModelError(string.Empty, "Erro: Já existe um Estado com este Nome ou UF cadastrado para este País!");
                CarregarListas();
                return Page();
            }

            bool sucesso = _estadoRepository.Inserir(Estado);
            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Estado cadastrado com sucesso!";

                return RedirectToPage("/EstadosListar");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Erro ao salvar o estado no banco de dados.");
                CarregarListas();
                return Page();
            }
        }

        private void CarregarListas()
        {
            ListaPaisesDisponiveis = _paisRepository.ListarTodos();
        }


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
            if (id <= 0 || string.IsNullOrEmpty(paisNome)) return new JsonResult(new { sucesso = false, mensagem = "Dados inválidos." });

            var paisEditado = new PaisModel { IdPais = id, Pais = paisNome, Sigla = paisSigla, Ddi = paisDdi, Moeda = paisMoeda };
            bool sucesso = _paisRepository.Atualizar(paisEditado); // Certifique-se de ter um método Atualizar no repositório

            if (sucesso) return new JsonResult(new { sucesso = true });
            return new JsonResult(new { sucesso = false, mensagem = "Erro ao atualizar." });
        }


        public JsonResult OnPostExcluirPaisRapido(int id)
        {
            if (id <= 0) return new JsonResult(new { sucesso = false });

            bool sucesso = _paisRepository.Excluir(id); // Certifique-se de ter um método Excluir no repositório

            if (sucesso) return new JsonResult(new { sucesso = true });
            // Se falhar, provavelmente é porque o país já está vinculado a um estado/fornecedor (Foreign Key)
            return new JsonResult(new { sucesso = false, mensagem = "Não é possível excluir este país pois ele está em uso." });
        }
    }
}
