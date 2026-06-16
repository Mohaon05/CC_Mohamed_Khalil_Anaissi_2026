using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    // NOVO: Arquivo dedicado exclusivamente para carregar a lista do banco
    public class PaisesListarModel : PageModel
    {
        private readonly PaisRepository _paisRepository;

        public PaisesListarModel(PaisRepository paisRepository)
        {
            _paisRepository = paisRepository;
        }

        public List<PaisModel> ListaPaises { get; set; } = new List<PaisModel>();

        public void OnGet()
        {
            // Quando a página abre, busca todos os países
            ListaPaises = _paisRepository.ListarTodos();
        }
    }
}