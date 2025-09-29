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
        private readonly PageRepository _pageRepo;

        public HomeController(ActivityDetailRepository activityDetailRepo, PageRepository pageRepo)
        {
            _activityDetailRepo = activityDetailRepo;
            _pageRepo = pageRepo;
        }

        public IActionResult Index(long pageId)
        {
            List<ActivityDetail> activities = _activityDetailRepo.GetAll("SELECTALL")
                .Where(a => a.PageMasterId == pageId).ToList();

            ViewBag.Page = _pageRepo.GetById(pageId, "SELECTBYID");
            return View(activities);
        }

        public IActionResult CreateActivityDetail()
        {
            BindDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateActivityDetail(ActivityDetail model)
        {
            if (ModelState.IsValid)
            {
                var result = _activityDetailRepo.Insert(model, "INSERT");
                if (result > 0)
                {
                    TempData["Success"] = "Activity detail created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Something went wrong!";
            }
            BindDropdowns();
            return View(model);
        }

        private void BindDropdowns()
        {
            ViewBag.Activities = _activityDetailRepo.GetAll("SELECTALL")
                                    .Select(x => new SelectListItem
                                    {
                                        Value = x.ID.ToString(),
                                        Text = x.Title
                                    }).ToList();

            ViewBag.Pages = _pageRepo.GetAll("SELECTALL")
                                    .Select(x => new SelectListItem
                                    {
                                        Value = x.ID.ToString(),
                                        Text = x.Title
                                    }).ToList();
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
