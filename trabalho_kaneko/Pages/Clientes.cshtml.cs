using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class ClientesModel : PageModel
    {
        private readonly ClienteRepository _clienteRepository;

        // O C# injeta o repositório automaticamente aqui
        public ClientesModel(ClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        // Essa propriedade segura os dados digitados na tela
        [BindProperty]
        public ClienteModel Cliente { get; set; }

        // Executado quando a página carrega pela primeira vez
        public void OnGet()
        {
        }

        // Executado quando o usuário clica no botão "Salvar" (Post)
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Se houver erro de validação, recarrega a página mostrando os erros
            }

            // Força o ID da cidade como 1 temporariamente para não dar erro de Chave Estrangeira
            Cliente.IdCidade = 1;

            bool sucesso = _clienteRepository.Inserir(Cliente);

            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Cliente cadastrado com sucesso!";
                return RedirectToPage("Clientes"); // Recarrega a página limpa
            }

            ModelState.AddModelError("", "Erro ao salvar no banco de dados.");
            return Page();
        }
    }
}