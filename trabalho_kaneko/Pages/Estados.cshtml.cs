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

        // MODIFICADO: Removemos a ListaEstados daqui. 
        // Mantemos APENAS a lista de países para o campo de seleção (Dropdown) funcionar.
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

                // MODIFICADO: Redireciona para a nova tela de listagem de estados
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
            // MODIFICADO: Carrega apenas os países
            ListaPaisesDisponiveis = _paisRepository.ListarTodos();
        }
    }
}
