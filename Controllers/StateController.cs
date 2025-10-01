using Ext_Dynamic_Form.Repository;
using Microsoft.AspNetCore.Mvc;
using Ext_Dynamic_Form.Models;

namespace Ext_Dynamic_Form.Controllers
{
    public class StateController : Controller
    {
        private readonly StateRepository _stateRepo;
        private readonly CountryRepository _countryRepo;

        public StateController(IConfiguration configuration)
        {
            _stateRepo = new StateRepository(configuration);
            _countryRepo = new CountryRepository(configuration);
        }

        public IActionResult Index()
        {
            var states = _stateRepo.GetAll("SELECTALL");
            return View(states);
        }

        public IActionResult Create()
        {
            ViewBag.Countries = _countryRepo.GetAll("SELECTALL");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(State state)
        {
            if (ModelState.IsValid)
            {
                _stateRepo.Insert(state, "INSERT");
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Countries = _countryRepo.GetAll("SELECTALL");
            return View(state);
        }

        public IActionResult Edit(long id)
        {
            var state = _stateRepo.GetById(id, "SELECTBYID");
            if (state == null) return NotFound();

            ViewBag.Countries = _countryRepo.GetAll("SELECTALL");
            return View(state);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(State state)
        {
            if (ModelState.IsValid)
            {
                _stateRepo.Insert(state, "UPDATE");
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Countries = _countryRepo.GetAll("SELECTALL");
            return View(state);
        }

        public IActionResult Delete(long id)
        {
            _stateRepo.Delete(id, "DELETE");
            return RedirectToAction(nameof(Index));
        }
    }
}
