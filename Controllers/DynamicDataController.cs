using Ext_Dynamic_Form.Models;
using Ext_Dynamic_Form.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Ext_Dynamic_Form.Controllers
{
    public class DynamicDataController : Controller
    {
        private readonly DynamicFormRepository _repo;

        public DynamicDataController(DynamicFormRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index(Int64? activityId)
        {
            var list = _repo.GetAll("SELECT", activityId);
            return View(list);
        }

       
        public IActionResult Create()
        {
            return View(new DynamicFormData());
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DynamicFormData model)
        {
            if (ModelState.IsValid)
            {
                _repo.InsertOrUpdate(model, "INSERT");
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        
        public IActionResult Edit(Int64 id)
        {
            var record = _repo.GetAll("SELECT").Find(x => x.ID == id);
            if (record == null) return NotFound();
            return View(record);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DynamicFormData model)
        {
            if (ModelState.IsValid)
            {
                _repo.InsertOrUpdate(model, "UPDATE");
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Delete(Int64 id)
        {
            _repo.Delete(id, "DELETE");
            return RedirectToAction("Index");
        }


    }
}
