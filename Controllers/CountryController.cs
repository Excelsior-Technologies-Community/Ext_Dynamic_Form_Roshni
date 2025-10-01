using Ext_Dynamic_Form.Models;
using Ext_Dynamic_Form.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Ext_Dynamic_Form.Controllers
{
    public class CountryController : Controller
    {
        private readonly CountryRepository _countryRepo;

        public CountryController(CountryRepository countryRepository)
        {
            _countryRepo = countryRepository;
        }

        public IActionResult Index()
        {
            var countries = _countryRepo.GetAll("SELECTALL"); 
            return View(countries);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Country country)
        {
            if (ModelState.IsValid)
            {
                _countryRepo.Insert(country, "INSERT");
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        public IActionResult Edit(long id)
        {
            var country = _countryRepo.GetById(id, "SELECTBYID");
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Country country)
        {
            if (ModelState.IsValid)
            {
                _countryRepo.Insert(country, "UPDATE");
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        
        public IActionResult Delete(long id)
        {
            var country = _countryRepo.GetById(id, "GETBYID");
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            _countryRepo.Delete(id, "DELETE");
            return RedirectToAction(nameof(Index));
        }
    }
}
