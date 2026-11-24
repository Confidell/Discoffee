using Discoffee.Models;
using Discoffee.Repository.Contract;
using Discoffee.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Discoffee.Areas.Gerencia.Controllers
{
    [Area("Gerencia")]
    [Authorize(Roles = "Administrador, Funcionario")]
    public class DashboardController : Controller
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        public DashboardController(IPagamentoRepository repository)
        {
            _pagamentoRepository = repository;
        }       

        public IActionResult Index()
        {
            List<Pagamento> pagamentos = _pagamentoRepository.VerTodos();

            DashboardViewModel dashboard = new DashboardViewModel
            {
                pagamentos = pagamentos
            };

            return View(dashboard);
        }
    }
}