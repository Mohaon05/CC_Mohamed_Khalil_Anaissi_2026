using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
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
            ListaPaises = _paisRepository.ListarTodos();
        }

        public IActionResult OnPostDeletar(int id)
        {
            bool sucesso = _paisRepository.Excluir(id);
            if (sucesso) TempData["MensagemSucesso"] = "País excluído com sucesso!";
            else TempData["MensagemErro"] = "Erro ao excluir o país. Verifique se ele está vinculado a estados.";
            return RedirectToPage();
        }
    }
}