using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabajo_kaneko.Repository;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class FormasPagamentoListarModel : PageModel
    {
        private readonly FormaPagamentoRepository _formaRepository;

        public FormasPagamentoListarModel(FormaPagamentoRepository formaRepository)
        {
            _formaRepository = formaRepository;
        }

        public List<FormaPagamentoModel> ListaFormas { get; set; } = new List<FormaPagamentoModel>();

        public void OnGet()
        {
            ListaFormas = _formaRepository.ListarTodos();
        }

        public IActionResult OnPostDeletar(int id)
        {
            bool sucesso = _formaRepository.Excluir(id);
            if (sucesso) TempData["MensagemSucesso"] = "Forma de pagamento excluída com sucesso!";
            else TempData["MensagemErro"] = "Não foi possível excluir. Verifique se ela já está vinculada a alguma venda/compra.";
            return RedirectToPage();
        }
    }
}