using ControleFinanceiroMvc.Models.ViewModels;
using ControleFinanceiroMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiroMvc.Controllers
{
    public class LancamentosController : Controller
    {
        private const int UsuarioIdPadrao = 1;
        private readonly ILancamentoService _lancamentoService;

        public LancamentosController(ILancamentoService lancamentoService)
        {
            _lancamentoService = lancamentoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] LancamentoFiltroViewModel filtro)
        {
            try
            {
                var viewModel = await _lancamentoService.ObterIndexAsync(UsuarioIdPadrao, filtro);
                return View(viewModel);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erro"] = ex.Message;
                return View(new LancamentosIndexViewModel { Filtro = filtro });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Novo()
        {
            try
            {
                var model = await _lancamentoService.CriarFormularioAsync(UsuarioIdPadrao);
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
        public async Task<IActionResult> Novo(LancamentoFormViewModel model)
        {
            try
            {
                model.Categorias = (await _lancamentoService.CriarFormularioAsync(UsuarioIdPadrao)).Categorias;
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var resultado = await _lancamentoService.CriarAsync(UsuarioIdPadrao, model);

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

            TempData["Sucesso"] = "Lançamento cadastrado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var model = await _lancamentoService.ObterFormularioEdicaoAsync(UsuarioIdPadrao, id);
                if (model is null)
                {
                    TempData["Erro"] = "Lançamento não encontrado.";
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
        public async Task<IActionResult> Editar(LancamentoFormViewModel model)
        {
            try
            {
                model.Categorias = (await _lancamentoService.CriarFormularioAsync(UsuarioIdPadrao)).Categorias;
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var resultado = await _lancamentoService.AtualizarAsync(UsuarioIdPadrao, model);

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

            TempData["Sucesso"] = "Lançamento atualizado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Excluir(int id)
        {
            try
            {
                var resultado = await _lancamentoService.ExcluirAsync(UsuarioIdPadrao, id);
                TempData[resultado.Sucesso ? "Sucesso" : "Erro"] =
                    resultado.Sucesso ? "Lançamento excluído com sucesso." : resultado.Erro;
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erro"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
