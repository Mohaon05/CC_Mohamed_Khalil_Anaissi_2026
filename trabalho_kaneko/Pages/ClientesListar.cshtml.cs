using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class ClientesListarModel : PageModel
    {
        private readonly ClienteRepository _clienteRepository;

        public ClientesListarModel(ClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public List<ClienteModel> ListaClientes { get; set; } = new List<ClienteModel>();

        public void OnGet()
        {
            ListaClientes = _clienteRepository.ListarTodos();
        }

        public IActionResult OnPostDeletar(int id)
        {
            bool sucesso = _clienteRepository.Excluir(id);

            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Cliente excluído com sucesso!";
            }
            else
            {
                TempData["MensagemErro"] = "Não foi possível excluir o cliente.";
            }

            return RedirectToPage();
        }
    }
}