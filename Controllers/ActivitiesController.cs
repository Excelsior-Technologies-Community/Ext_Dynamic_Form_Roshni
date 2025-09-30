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

        //public IActionResult Details(int id)
        //{
        //    var activity = _activityRepo.GetById(id, "SELECTBYID");
        //    if (activity == null)
        //    {
        //        return NotFound();
        //    }

        //    var details = _activityDetailRepo.GetActivityDetailsByActivityId(id, "SELECTBYACTIVITYID");

        //    ViewBag.ActivityTitle = activity.Title;
        //    return PartialView("ActivityDetailsPartial", details);
        //}

        public IActionResult Details(int id)
        {
            var activity = _activityRepo.GetById(id, "SELECTBYID");
            if (activity == null)
                return NotFound();

            var details = _activityDetailRepo.GetActivityDetailsByActivityId(id, "SELECTBYACTIVITYID");

            // Map ActionTypeId to name
            var actionTypeMap = new Dictionary<long, string>
            {
                { 1, "Drop-down" },
                { 2, "Text" },
                { 3, "Status" },
                { 4, "Number" },
                { 5, "Checkbox" },
                { 6, "Rating" },
                { 7, "TextArea" },
                { 8, "Date" },
                { 9, "Checkbox 123" },
                { 10, "File" }
            };


            var pages = _pageRepo.GetAll("SELECTALL");


            ViewBag.ActionTypeMap = actionTypeMap;
            ViewBag.Pages = pages.ToDictionary(p => p.ID, p => p.Title);

            ViewBag.ActivityTitle = activity.Title;

            return PartialView("ActivityDetailsPartial", details);
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
