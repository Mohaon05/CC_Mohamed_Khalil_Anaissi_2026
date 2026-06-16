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

        // NOVO: Trazemos o repositório de Estados para popular a caixa de seleção na tela
        private readonly EstadoRepository _estadoRepository;

        public CidadesModel(CidadeRepository cidadeRepository, EstadoRepository estadoRepository)
        {
            _cidadeRepository = cidadeRepository;
            _estadoRepository = estadoRepository;
        }

        [BindProperty]
        public CidadeModel Cidade { get; set; }

        public List<CidadeModel> ListaCidades { get; set; } = new List<CidadeModel>();

        // NOVO: Lista para o campo <select> de Estados
        public List<EstadoModel> ListaEstadosDisponiveis { get; set; } = new List<EstadoModel>();

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

            bool sucesso = _cidadeRepository.Inserir(Cidade);

            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Cidade cadastrada com sucesso!";
                return RedirectToPage("/Cidades");
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
            ListaCidades = _cidadeRepository.ListarTodos();
            ListaEstadosDisponiveis = _estadoRepository.ListarTodos();
        }
    }
}