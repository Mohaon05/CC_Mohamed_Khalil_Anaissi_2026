using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class FornecedoresModel : PageModel
    {
        // 1. INJEÇÃO DOS REPOSITÓRIOS
        private readonly FornecedorRepository _fornecedorRepository;
        private readonly CidadeRepository _cidadeRepository;
        private readonly EstadoRepository _estadoRepository;
        private readonly PaisRepository _paisRepository;

        // 2. CONSTRUTOR
        public FornecedoresModel(
            FornecedorRepository fornecedorRepository,
            CidadeRepository cidadeRepository,
            EstadoRepository estadoRepository,
            PaisRepository paisRepository)
        {
            _fornecedorRepository = fornecedorRepository;
            _cidadeRepository = cidadeRepository;
            _estadoRepository = estadoRepository;
            _paisRepository = paisRepository;
        }

        [BindProperty]
        public FornecedorModel Fornecedor { get; set; }

        // 3. DECLARAÇÃO DAS LISTAS
        public List<CidadeModel> ListaCidades { get; set; } = new List<CidadeModel>();
        public List<EstadoModel> ListaEstados { get; set; } = new List<EstadoModel>();
        public List<PaisModel> ListaPaises { get; set; } = new List<PaisModel>();

        public void OnGet()
        {
            ListaCidades = _cidadeRepository.ListarTodos();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ListaCidades = _cidadeRepository.ListarTodos();
                return Page();
            }

            bool sucesso = _fornecedorRepository.Inserir(Fornecedor);
            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Fornecedor cadastrado com sucesso!";
                return RedirectToPage("/FornecedoresListar");
            }

            ModelState.AddModelError(string.Empty, "Erro ao salvar. Verifique se o CPF/CNPJ já existe.");
            ListaCidades = _cidadeRepository.ListarTodos();
            return Page();
        }
    }
}