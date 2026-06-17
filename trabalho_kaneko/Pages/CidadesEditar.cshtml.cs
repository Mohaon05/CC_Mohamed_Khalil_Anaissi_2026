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

        public CidadesEditarModel(CidadeRepository cidadeRepository, EstadoRepository estadoRepository)
        {
            _cidadeRepository = cidadeRepository;
            _estadoRepository = estadoRepository;
        }

        [BindProperty]
        public CidadeModel Cidade { get; set; }

        public List<EstadoModel> ListaEstadosDisponiveis { get; set; } = new List<EstadoModel>();

        public IActionResult OnGet(int id)
        {
            Cidade = _cidadeRepository.BuscarPorId(id);

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
            ListaEstadosDisponiveis = _estadoRepository.ListarTodos();
        }
    }
}