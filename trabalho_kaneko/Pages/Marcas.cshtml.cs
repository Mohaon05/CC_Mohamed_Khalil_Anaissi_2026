using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using trabalho_kaneko.Models;
using trabalho_kaneko.Repository;

namespace trabalho_kaneko.Pages
{
    public class MarcasModel : PageModel
    {
        private readonly MarcaRepository _marcaRepository;
        private readonly GrupoRepository _grupoRepository;

        public MarcasModel(MarcaRepository marcaRepository, GrupoRepository grupoRepository)
        {
            _marcaRepository = marcaRepository;
            _grupoRepository = grupoRepository;
        }

        [BindProperty]
        public MarcaModel MarcaObj { get; set; }

        public List<GrupoModel> ListaGrupos { get; set; } = new List<GrupoModel>();

        public void OnGet()
        {
            ListaGrupos = _grupoRepository.ListarTodos();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ListaGrupos = _grupoRepository.ListarTodos();
                return Page();
            }

            bool sucesso = _marcaRepository.Inserir(MarcaObj);
            if (sucesso)
            {
                TempData["MensagemSucesso"] = "Marca cadastrada com sucesso!";
                return RedirectToPage("/MarcasListar");
            }

            ModelState.AddModelError(string.Empty, "Erro ao salvar a marca.");
            ListaGrupos = _grupoRepository.ListarTodos();
            return Page();
        }
    }
}