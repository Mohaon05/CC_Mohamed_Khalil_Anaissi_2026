using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class CondicoesPagamentoEditarModel : PageModel
    {
        private readonly CondicaoPagamentoRepository _condicaoRepository;

        public CondicoesPagamentoEditarModel(CondicaoPagamentoRepository condicaoRepository)
        {
            _condicaoRepository = condicaoRepository;
        }

        [BindProperty]
        public CondicaoPagamentoModel CondicaoObj { get; set; }

        // Estrutura preparada para quando tiver a tabela de parcelas no banco:
        // [BindProperty]
        // public List<ParcelaModel> Parcelas { get; set; } = new List<ParcelaModel>();

        public IActionResult OnGet(int id)
        {
            CondicaoObj = _condicaoRepository.BuscarPorId(id);

            if (CondicaoObj == null)
            {
                TempData["MensagemErro"] = "Condição de pagamento não encontrada.";
                return RedirectToPage("/CondicoesPagamentoListar");
            }

            // Futuramente: carregar as parcelas existentes para exibição no frontend
            // Parcelas = _condicaoRepository.ListarParcelasPorCondicao(id);

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool sucesso = _condicaoRepository.Atualizar(CondicaoObj);

            if (sucesso)
            {
                // Futuramente: lógica para apagar as parcelas antigas e inserir as novas aqui

                TempData["MensagemSucesso"] = "Condição de pagamento atualizada com sucesso!";
                return RedirectToPage("/CondicoesPagamentoListar");
            }

            TempData["MensagemErro"] = "Erro ao atualizar a condição de pagamento na base de dados.";
            return Page();
        }
    }
}