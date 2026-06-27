using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class CondicoesPagamentoListarModel : PageModel
    {
        private readonly CondicaoPagamentoRepository _condicaoRepository;

        public CondicoesPagamentoListarModel(CondicaoPagamentoRepository condicaoRepository)
        {
            _condicaoRepository = condicaoRepository;
        }

        public List<CondicaoPagamentoModel> ListaCondicoes { get; set; } = new List<CondicaoPagamentoModel>();

        public void OnGet()
        {
            ListaCondicoes = _condicaoRepository.ListarTodos();
        }

        public IActionResult OnPostDeletar(int id)
        {
            bool sucesso = _condicaoRepository.Excluir(id);
            if (sucesso) TempData["MensagemSucesso"] = "Condição de pagamento excluída!";
            else TempData["MensagemErro"] = "Não foi possível excluir. Verifique se existem parcelas vinculadas.";
            return RedirectToPage();
        }
    }
}