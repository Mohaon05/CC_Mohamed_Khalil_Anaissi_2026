using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class MarcasEditarModel : PageModel
    {
        private readonly MarcaRepository _marcaRepository;
        private readonly GrupoRepository _grupoRepository;

        public MarcasEditarModel(MarcaRepository marcaRepository, GrupoRepository grupoRepository)
        {
            _marcaRepository = marcaRepository;
            _grupoRepository = grupoRepository;
        }

        [BindProperty]
        public MarcaModel MarcaObj { get; set; }

        public List<GrupoModel> ListaGrupos { get; set; } = new List<GrupoModel>();

        public IActionResult OnGet(int id)
        {
            MarcaObj = _marcaRepository.BuscarPorId(id);
            if (MarcaObj == null)
            {
                return RedirectToPage("/MarcasListar");
            }
            ListaGrupos = _grupoRepository.ListarTodos();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ListaGrupos = _grupoRepository.ListarTodos();
                return Page();
            }

            bool sucesso = _marcaRepository.Atualizar(MarcaObj);
            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Marca atualizada com sucesso!";
                return RedirectToPage("/MarcasListar");
            }

            ModelState.AddModelError(string.Empty, "Erro ao atualizar.");
            ListaGrupos = _grupoRepository.ListarTodos();
            return Page();
        }
    }
}