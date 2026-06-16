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

        public CidadesModel(CidadeRepository cidadeRepository, EstadoRepository estadoRepository)
        {
            _cidadeRepository = cidadeRepository;
            _estadoRepository = estadoRepository;
        }

        [BindProperty]
        public CidadeModel Cidade { get; set; }

        // MODIFICADO: Removemos a ListaCidades daqui.
        // Mantemos APENAS os estados para preencher o campo de seleção (Dropdown).
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
            ListaEstadosDisponiveis = _estadoRepository.ListarTodos();
        }
    }
}