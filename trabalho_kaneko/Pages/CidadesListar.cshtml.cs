using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    // NOVO: Arquivo criado exclusivamente para gerenciar a consulta de cidades
    public class CidadesListarModel : PageModel
    {
        private readonly CidadeRepository _cidadeRepository;

        public CidadesListarModel(CidadeRepository cidadeRepository)
        {
            _cidadeRepository = cidadeRepository;
        }

        public List<CidadeModel> ListaCidades { get; set; } = new List<CidadeModel>();

        public void OnGet()
        {
            // Busca as cidades trazendo o JOIN com os estados
            ListaCidades = _cidadeRepository.ListarTodos();
        }
    }
}