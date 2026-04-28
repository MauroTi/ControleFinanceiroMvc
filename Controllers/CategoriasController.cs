using ControleFinanceiroMvc.Models.ViewModels;
using ControleFinanceiroMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiroMvc.Controllers
{
    public class CategoriasController : Controller
    {
        private const int UsuarioIdPadrao = 1;
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var categorias = await _categoriaService.ListarPorUsuarioIdAsync(UsuarioIdPadrao);
                return View(categorias);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erro"] = ex.Message;
                return View(Array.Empty<CategoriaViewModel>());
            }
        }

        [HttpGet]
        public IActionResult Nova()
        {
            return View(new CategoriaViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Nova(CategoriaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            (bool Sucesso, string? Erro) resultado;

            try
            {
                resultado = await _categoriaService.CriarAsync(UsuarioIdPadrao, model);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }

            if (!resultado.Sucesso)
            {
                ModelState.AddModelError(string.Empty, resultado.Erro!);
                return View(model);
            }

            TempData["Sucesso"] = "Categoria cadastrada com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var model = await _categoriaService.ObterParaEdicaoAsync(UsuarioIdPadrao, id);
                if (model is null)
                {
                    TempData["Erro"] = "Categoria não encontrada.";
                    return RedirectToAction(nameof(Index));
                }

                return View(model);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erro"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(CategoriaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var resultado = await _categoriaService.AtualizarAsync(UsuarioIdPadrao, model);

                if (!resultado.Sucesso)
                {
                    ModelState.AddModelError(string.Empty, resultado.Erro!);
                    return View(model);
                }
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }

            TempData["Sucesso"] = "Categoria atualizada com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Excluir(int id)
        {
            try
            {
                var resultado = await _categoriaService.ExcluirAsync(UsuarioIdPadrao, id);
                TempData[resultado.Sucesso ? "Sucesso" : "Erro"] =
                    resultado.Sucesso ? "Categoria excluída com sucesso." : resultado.Erro;
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erro"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
