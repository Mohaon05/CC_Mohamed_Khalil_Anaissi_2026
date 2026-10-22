using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class ComprasListarModel : PageModel
    {
        private readonly CompraRepository _compraRepository;

        public ComprasListarModel(CompraRepository compraRepository)
        {
            _compraRepository = compraRepository;
        }

        public List<CompraModel> ListaCompras { get; set; } = new List<CompraModel>();

        public void OnGet()
        {
            // Busca todos os registros do banco de dados ao carregar a página
            ListaCompras = _compraRepository.ListarTodos();
        }

        public IActionResult OnPostExcluir(string modelo, string serie, int numNfe, int idFornecedor)
        {
            // Como a PK é composta por 4 campos, precisamos passar os 4 para excluir
            bool sucesso = _compraRepository.Excluir(modelo, serie, numNfe, idFornecedor);

            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Nota Fiscal excluída com sucesso!";
            }
            else
            {
                TempData["MensagemErro"] = "Erro ao excluir a Nota Fiscal.";
            }

            return RedirectToPage("/ComprasListar");
        }
    }
}