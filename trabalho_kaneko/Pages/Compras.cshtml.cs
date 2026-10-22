using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class ComprasModel : PageModel
    {
        private readonly CompraRepository _compraRepository;
        private readonly FornecedorRepository _fornecedorRepository;
        private readonly CondicaoPagamentoRepository _condicaoRepository;

        public ComprasModel(CompraRepository compraRepository, FornecedorRepository fornecedorRepository, CondicaoPagamentoRepository condicaoRepository)
        {
            _compraRepository = compraRepository;
            _fornecedorRepository = fornecedorRepository;
            _condicaoRepository = condicaoRepository;
        }

        [BindProperty]
        public CompraModel CompraObj { get; set; }

        public List<FornecedorModel> ListaFornecedores { get; set; } = new List<FornecedorModel>();
        // Assumindo que você tem CondicaoPagamentoModel implementado
        public List<CondicaoPagamentoModel> ListaCondicoes { get; set; } = new List<CondicaoPagamentoModel>();

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

            bool sucesso = _compraRepository.Inserir(CompraObj);

            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Nota Fiscal de Compra lançada com sucesso!";
                return RedirectToPage("/ComprasListar"); // Crie essa página depois
            }

            ModelState.AddModelError(string.Empty, "Erro ao salvar a Nota Fiscal. Verifique se a chave composta já não existe.");
            CarregarListas();
            return Page();
        }

        private void CarregarListas()
        {
            ListaFornecedores = _fornecedorRepository.ListarTodos();
            ListaCondicoes = _condicaoRepository.ListarTodos(); // Crie este método no seu repositório
        }
    }
}