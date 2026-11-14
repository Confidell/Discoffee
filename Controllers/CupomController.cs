using Microsoft.AspNetCore.Mvc;
using Discoffee.Models;
using Discoffee.Repository.Contract;

namespace Discoffee.Controllers
{
    public class CupomController : Controller
    {
        private readonly ICupomRepository _cupomRepository;

        public CupomController(ICupomRepository cupomRepository)
        {
            _cupomRepository = cupomRepository;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Cupom cupom)
        {
            if (_cupomRepository.BuscarCupomPorCodigo(cupom.Codigo) != null)
            {
                ModelState.AddModelError(nameof(cupom.Codigo), "Já existe um cupom com esse código.");
            }

            if (ModelState.IsValid)
            {
                _cupomRepository.AdicionarCupom(cupom);
                return RedirectToAction(nameof(Create));
            }

            return View(cupom);
        }

        public IActionResult Edit(string codigo)
        {
            var cupom = _cupomRepository.BuscarCupomPorCodigo(codigo);
            if (cupom == null) return NotFound();

            return View(cupom);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Cupom cupom)
        {
            var existente = _cupomRepository.BuscarCupomPorCodigo(cupom.Codigo);
            if (existente != null && existente.IdCupom != cupom.IdCupom)
            {
                ModelState.AddModelError(nameof(cupom.Codigo), "Já existe outro cupom com esse código.");
            }

            if (ModelState.IsValid)
            {
                _cupomRepository.EditarCupom(cupom);
                return RedirectToAction(nameof(Edit), new { codigo = cupom.Codigo });
            }

            return View(cupom);
        }

        public IActionResult Delete(string codigo)
        {
            var cupom = _cupomRepository.BuscarCupomPorCodigo(codigo);
            if (cupom == null) return NotFound();

            return View(cupom);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _cupomRepository.ExcluirCupom(id);
            return RedirectToAction(nameof(Create));
        }
    }
}

