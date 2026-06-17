using Microsoft.AspNetCore.Mvc;
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

        // NOVO: Executado quando o formulário de exclusão for enviado
        public IActionResult OnPostDeletar(int id)
        {
            bool sucesso = _paisRepository.Excluir(id);

            if (sucesso)
            {
                TempData["MensagemSucesso"] = "País excluído com sucesso!";
            }
            else
            {
                // Mensagem de aviso caso o banco bloqueie a exclusão
                TempData["MensagemErro"] = "Não foi possível excluir o país. Verifique se ele não possui estados vinculados.";
            }

            // Recarrega a própria página de listagem para atualizar a tabela
            return RedirectToPage();
        }

    }
}