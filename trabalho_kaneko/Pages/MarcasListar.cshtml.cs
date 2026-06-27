using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class MarcasListarModel : PageModel
    {
        private readonly MarcaRepository _marcaRepository;

        public MarcasListarModel(MarcaRepository marcaRepository)
        {
            _marcaRepository = marcaRepository;
        }

        public List<MarcaModel> ListaMarcas { get; set; } = new List<MarcaModel>();

        public void OnGet()
        {
            ListaMarcas = _marcaRepository.ListarTodos();
        }

        public IActionResult OnPostDeletar(int id)
        {
            bool sucesso = _marcaRepository.Excluir(id);
            if (sucesso) TempData["MensagemSucesso"] = "Marca excluída com sucesso!";
            else TempData["MensagemErro"] = "Não foi possível excluir. Verifique se a marca está sendo usada.";
            return RedirectToPage();
        }
    }
}