using Ext_Dynamic_Form.Models;
using Ext_Dynamic_Form.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ext_Dynamic_Form.Controllers
{
    public class CityController : Controller
    {
        private readonly CityRepository _cityRepo;
        private readonly StateRepository _stateRepo;
        private readonly CountryRepository _countryRepo;

        public CityController(IConfiguration configuration)
        {
            _cityRepo = new CityRepository(configuration);
            _stateRepo = new StateRepository(configuration);
            _countryRepo = new CountryRepository(configuration);
        }

        public IActionResult Index()
        {
            var cities = _cityRepo.GetAll("SELECTALL");
            return View(cities);
        }

        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(City city)
        {
            if (ModelState.IsValid)
            {
                _cityRepo.Insert(city, "INSERT");
                return RedirectToAction(nameof(Index));
            }
            LoadDropdowns();
            return View(city);
        }

        public IActionResult Edit(long id)
        {
            var city = _cityRepo.GetById(id, "SELECTBYID");
            if (city == null) return NotFound();

            LoadDropdowns(city.CountryId, city.StateId);
            return View(city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(City city)
        {
            if (ModelState.IsValid)
            {
                _cityRepo.Insert(city, "UPDATE");
                return RedirectToAction(nameof(Index));
            }
            LoadDropdowns(city.CountryId, city.StateId);
            return View(city);
        }

        public IActionResult Delete(long id)
        {
            _cityRepo.Delete(id, "DELETE");
            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns(long? countryId = null, long? stateId = null)
        {
            ViewBag.Countries = new SelectList(_countryRepo.GetAll("SELECTALL"), "ID", "CountryName", countryId);
            ViewBag.States = new SelectList(_stateRepo.GetAll("SELECTALL"), "ID", "StateName", stateId);
        }


    }
}
