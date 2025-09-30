using Ext_Dynamic_Form.Models;
using Ext_Dynamic_Form.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Ext_Dynamic_Form.Controllers
{
    public class ActivityDetailsController : Controller
    {
        private readonly ActivityDetailRepository _activityDetailRepo;
        private readonly ActivityRepository _activityRepo;
        private readonly PageRepository _pageRepo;

        public ActivityDetailsController(ActivityRepository activityRepo,ActivityDetailRepository activityDetailRepository,
            PageRepository pageRepo)
        {
            _activityDetailRepo = activityDetailRepository;
            _activityRepo = activityRepo;
            _pageRepo = pageRepo;
        }

        public IActionResult Index()
        {
            var details = _activityDetailRepo.GetAll("SELECTALL");
            return View(details);
        }

        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ActivityDetail detail)
        {
            if (ModelState.IsValid)
            {
                _activityDetailRepo.Insert(detail, "INSERT");
                return RedirectToAction(nameof(Index));
            }
            LoadDropdowns();
            return View(detail);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(ActivityDetail model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        foreach (var field in model.Details)
        //        {
        //            var detail = new ActivityDetail
        //            {
        //                ActivityId = model.ActivityId,
        //                PageMasterId = model.PageMasterId,
        //                Title = field.Title,
        //                ActionTypeId = field.ActionTypeId
        //            };
        //            _activityDetailRepo.Insert(detail, "INSERT");
        //        }

        //        return RedirectToAction(nameof(Index));
        //    }

        //    LoadDropdowns();
        //    return View(model);
        //}



        public IActionResult Details(int id)
        {
            var activity = _activityRepo.GetById(id, "SELECTBYID");
            if (activity == null)
            {
                return NotFound();
            }

            var details = _activityDetailRepo.GetActivityDetailsByActivityId(id, "SELECTBYID");

            ViewBag.ActivityTitle = activity.Title;
            return View(details);
        }


        public IActionResult Edit(long id)
        {
            var detail = _activityDetailRepo.GetById(id, "GETBYID");
            if (detail == null)
            {
                return NotFound();
            }

            LoadDropdowns();
            return View(detail);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, ActivityDetail detail)
        {
            if (id != detail.ID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _activityDetailRepo.Update(detail, "UPDATE");
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns();
            return View(detail);
        }

        
        public IActionResult Delete(long id)
        {
            var detail = _activityDetailRepo.GetById(id, "GETBYID");
            if (detail == null)
            {
                return NotFound();
            }
            return View(detail);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            _activityDetailRepo.Delete(id, "DELETE");
            return RedirectToAction(nameof(Index));
        }

        
        private void LoadDropdowns()
        {
            ViewBag.Activities = _activityRepo.GetAll("SELECTALL");
            ViewBag.Pages = _pageRepo.GetAll("SELECTALL");
        }


    }
}
