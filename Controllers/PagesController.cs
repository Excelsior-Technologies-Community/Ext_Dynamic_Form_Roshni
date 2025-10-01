using Ext_Dynamic_Form.Repository;
using Microsoft.AspNetCore.Mvc;
using Ext_Dynamic_Form.Models;

namespace Ext_Dynamic_Form.Controllers
{

    public class PagesController : Controller
    {
        private readonly ActivityDetailRepository _activityDetailRepo;
        private readonly ActivityRepository _activityRepo;
        private readonly PageRepository _pageRepo;

        public PagesController(ActivityRepository activityRepo, ActivityDetailRepository activityDetailRepository,
            PageRepository pageRepo)
        {
            _activityDetailRepo = activityDetailRepository;
            _activityRepo = activityRepo;
            _pageRepo = pageRepo;
        }

        public IActionResult Index()
        {
            var pages = _pageRepo.GetAll("SELECTALL");
            return View(pages);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Page page)
        {
            if (ModelState.IsValid)
            {
                _pageRepo.Insert(page, "INSERT");
                return RedirectToAction(nameof(Index));
            }
            return View(page);
        }

       
        public IActionResult Edit(long id)
        {
            var page = _pageRepo.GetById(id, "SELECTBYID");
            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, Page page)
        {
            if (id != page.ID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _pageRepo.Insert(page, "UPDATE"); 
                return RedirectToAction(nameof(Index));
            }
            return View(page);
        }

        public IActionResult Delete(long id)
        {
            var page = _pageRepo.GetById(id, "SELECTBYID");
            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            _pageRepo.Delete(id, "DELETE");
            return RedirectToAction(nameof(Index));
        }

    }
}
