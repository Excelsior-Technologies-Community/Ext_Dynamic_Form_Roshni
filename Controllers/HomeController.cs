using Ext_Dynamic_Form.Models;
using Ext_Dynamic_Form.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using SysActivity = System.Diagnostics.Activity;

namespace Ext_Dynamic_Form.Controllers
{
    public class HomeController : Controller
    {
        private readonly ActivityDetailRepository _activityDetailRepo;
        private readonly ActivityRepository _activityRepository;
        private readonly PageRepository _pageRepo;

        public HomeController(ActivityDetailRepository activityDetailRepo,ActivityRepository activityRepository, PageRepository pageRepo)
        {
            _activityDetailRepo = activityDetailRepo;
            _activityRepository = activityRepository;
            _pageRepo = pageRepo;
        }

        public IActionResult Index(long pageId)
        {
            List<ActivityDetail> activities = _activityDetailRepo.GetAll("SELECTALL")
                .Where(a => a.PageMasterId == pageId).ToList();

            ViewBag.Page = _pageRepo.GetById(pageId, "SELECTBYID");
            return View(activities);
        }


        public IActionResult DynamicForm()
        {
            var activities = _activityRepository.GetAll("SELECTALL");
            ViewBag.Activities = activities;
            return View();
        }

        public IActionResult Form()
        {
            var activities = _activityRepository.GetAll("SELECTALL");
            ViewBag.Activities = activities;
            return View();
        }


        [HttpGet]
        public IActionResult GetFormByActivityId(int activityId)
        {
            var details = _activityDetailRepo.GetActivityDetailsByActivityId(activityId, "SELECTBYACTIVITYID");

            if (details == null || !details.Any())
            {
                return PartialView("_DynamicFormPartial", new List<ActivityDetail>()); // return empty
            }

            var activity = _activityRepository.GetById(activityId, "SELECTBYID");
            ViewBag.ActivityTitle = activity?.Title ?? "Dynamic Form";

            return PartialView("_DynamicFormPartial", details);
        }

        [HttpPost]
        public IActionResult RegisterSubmit(int ActivityId, IFormCollection form)
        {
            foreach (var key in form.Keys)
            {
                if (key.StartsWith("Field_"))
                {
                    var value = form[key];
                    
                }
            }

            TempData["Success"] = "Form submitted successfully!";
            return RedirectToAction("DynamicForm");
        }


        [HttpPost]
        public IActionResult dynamicForm(int ActivityId, IFormCollection form)
        {
            foreach (var key in form.Keys)
            {
                if (key.StartsWith("Field_"))
                {
                    var value = form[key];

                }
            }

            TempData["Success"] = "Form submitted successfully!";
            return RedirectToAction("Form");
        }

        public IActionResult PageMaster()
        {
            var pages = _pageRepo.GetAll("SELECTALL");
            return View(pages);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = SysActivity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
