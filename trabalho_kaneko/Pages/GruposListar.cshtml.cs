using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class GruposListarModel : PageModel
    {
        private readonly GrupoRepository _grupoRepository;

        public GruposListarModel(GrupoRepository grupoRepository)
        {
            _grupoRepository = grupoRepository;
        }

        public List<GrupoModel> ListaGrupos { get; set; } = new List<GrupoModel>();

        public void OnGet()
        {
            ListaGrupos = _grupoRepository.ListarTodos();
        }

        public IActionResult OnPostDeletar(int id)
        {
            bool sucesso = _grupoRepository.Excluir(id);
            if (sucesso) TempData["MensagemSucesso"] = "Grupo excluído com sucesso!";
            else TempData["MensagemErro"] = "Não foi possível excluir o grupo. Verifique se ele está sendo usado em algum produto.";
            return RedirectToPage();
        }
    }
}