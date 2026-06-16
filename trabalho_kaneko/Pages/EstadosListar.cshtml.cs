using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    // NOVO: Arquivo dedicado exclusivamente para puxar os Estados do banco
    public class EstadosListarModel : PageModel
    {
        private readonly EstadoRepository _estadoRepository;

        public EstadosListarModel(EstadoRepository estadoRepository)
        {
            _estadoRepository = estadoRepository;
        }

        public List<EstadoModel> ListaEstados { get; set; } = new List<EstadoModel>();

        public void OnGet()
        {
            ListaEstados = _estadoRepository.ListarTodos();
        }
    }
}