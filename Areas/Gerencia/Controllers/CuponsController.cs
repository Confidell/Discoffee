using Discoffee.Models;
using Discoffee.Models.Constants;
using Discoffee.Repository;
using Discoffee.Repository.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Discoffee.Areas.Gerencia.Controllers
{
    [Area("Gerencia")]
    [Authorize(Roles = "Administrador, Funcionario")]
    public class CuponsController : Controller
    {
        private readonly ICupomRepository _cupomRepository;
        public CuponsController( ICupomRepository cupomRepository)
        {
            _cupomRepository = cupomRepository;
        }

        public IActionResult Index()
        {
            var cupons = _cupomRepository.BuscarTodosCupons();
            return View(cupons);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Excluir(int idCupom)
        {
            _cupomRepository.ExcluirCupom(idCupom);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Cadastrar(Cupom cupom, string desconto)
        {
            cupom.Desconto = decimal.Parse(desconto.Replace(',', '.'), CultureInfo.InvariantCulture);
            _cupomRepository.AdicionarCupom(cupom);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public IActionResult Editar(Cupom cupom, string desconto)
        {
            cupom.Desconto = decimal.Parse(desconto.Replace(',', '.'), CultureInfo.InvariantCulture);
            _cupomRepository.EditarCupom(cupom);
            return RedirectToAction(nameof(Index));
        }


    }
}
