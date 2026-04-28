using System.Diagnostics;
using ControleFinanceiroMvc.Models;
using ControleFinanceiroMvc.Models.ViewModels;
using ControleFinanceiroMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiroMvc.Controllers
{
    public class HomeController : Controller
    {
        private const int UsuarioIdPadrao = 1;
        private readonly ILogger<HomeController> _logger;
        private readonly ILancamentoService _lancamentoService;

        public HomeController(ILogger<HomeController> logger, ILancamentoService lancamentoService)
        {
            _logger = logger;
            _lancamentoService = lancamentoService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var indexLancamentos = await _lancamentoService.ObterIndexAsync(UsuarioIdPadrao, new LancamentoFiltroViewModel());

                var dashboard = new DashboardViewModel
                {
                    Resumo = indexLancamentos.Resumo,
                    UltimosLancamentos = indexLancamentos.Lancamentos.Take(5).ToList()
                };

                return View(dashboard);
            }
            catch (InvalidOperationException ex)
            {
                return View(new DashboardViewModel { Aviso = ex.Message });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
