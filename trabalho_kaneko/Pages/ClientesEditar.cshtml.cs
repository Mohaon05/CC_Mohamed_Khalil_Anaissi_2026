using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class ClientesEditarModel : PageModel
    {
        private readonly ClienteRepository _clienteRepository;
        private readonly CidadeRepository _cidadeRepository;

        public ClientesEditarModel(ClienteRepository clienteRepository, CidadeRepository cidadeRepository)
        {
            _clienteRepository = clienteRepository;
            _cidadeRepository = cidadeRepository;
        }

        [BindProperty]
        public ClienteModel Cliente { get; set; }

        public List<CidadeModel> ListaCidades { get; set; } = new List<CidadeModel>();

        public IActionResult OnGet(int id)
        {
            // Puxa o cliente específico e a lista de cidades para o dropdown
            Cliente = _clienteRepository.BuscarPorId(id);
            ListaCidades = _cidadeRepository.ListarTodos();

            if (Cliente == null)
            {
                return RedirectToPage("/ClientesListar");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ListaCidades = _cidadeRepository.ListarTodos();
                return Page();
            }

            bool sucesso = _clienteRepository.Atualizar(Cliente);

            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Cliente atualizado com sucesso!";
                return RedirectToPage("/ClientesListar");
            }
            else
            {
                TempData["MensagemErro"] = "Erro ao atualizar o cliente no banco de dados.";
                ListaCidades = _cidadeRepository.ListarTodos();
                return Page();
            }
        }
    }
}