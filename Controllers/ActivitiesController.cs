using Ext_Dynamic_Form.Repository;
using Microsoft.AspNetCore.Mvc;
using Ext_Dynamic_Form.Models;
namespace Ext_Dynamic_Form.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ActivityDetailRepository _activityDetailRepo;
        private readonly ActivityRepository _activityRepo;
        private readonly PageRepository _pageRepo;

        public ActivitiesController(ActivityRepository activityRepo, ActivityDetailRepository activityDetailRepository,        
            PageRepository pageRepo)
        {
            _activityDetailRepo = activityDetailRepository;
            _activityRepo = activityRepo;
            _pageRepo = pageRepo;
        }

        public IActionResult Index()
        {
            var activities = _activityRepo.GetAll("SELECTALL");
            return View(activities);
        }

        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Activity activity)
        {
            if (ModelState.IsValid)
            {
                _activityRepo.Insert(activity, "INSERT");
                return RedirectToAction(nameof(Index));
            }
            return View(activity);
        }

        
        public IActionResult Edit(long id)
        {
            var activity = _activityRepo.GetById(id, "GETBYID");
            if (activity == null)
            {
                return NotFound();
            }
            return View(activity);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, Activity activity)
        {
            if (id != activity.ID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _activityRepo.Update(activity, "UPDATE");
                return RedirectToAction(nameof(Index));
            }
            return View(activity);
        }

        public IActionResult Delete(long id)
        {
            var activity = _activityRepo.GetById(id, "GETBYID");
            if (activity == null)
            {
                return NotFound();
            }
            return View(activity);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            _activityRepo.Delete(id, "DELETE");
            return RedirectToAction(nameof(Index));
        }
    }
}
