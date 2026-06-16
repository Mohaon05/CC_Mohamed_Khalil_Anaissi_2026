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
        private readonly PaisRepository _paisRepository; // NOVO: Injetamos o repositório de Países

        // NOVO: O construtor agora pede os dois repositórios
        public EstadosModel(EstadoRepository estadoRepository, PaisRepository paisRepository)
        {
            _estadoRepository = estadoRepository;
            _paisRepository = paisRepository;
        }

        [BindProperty]
        public EstadoModel Estado { get; set; }

        public List<EstadoModel> ListaEstados { get; set; } = new List<EstadoModel>();

        // NOVO: Lista criada especificamente para popular o campo "Select" de Países na tela
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

            bool sucesso = _estadoRepository.Inserir(Estado);

            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Estado cadastrado com sucesso!";
                return RedirectToPage("/Estados");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Erro ao salvar o estado no banco de dados.");
                CarregarListas();
                return Page();
            }
        }

        // NOVO: Um método auxiliar para não repetir código, carrega ambas as listas
        private void CarregarListas()
        {
            ListaEstados = _estadoRepository.ListarTodos();
            ListaPaisesDisponiveis = _paisRepository.ListarTodos();
        }
    }
}