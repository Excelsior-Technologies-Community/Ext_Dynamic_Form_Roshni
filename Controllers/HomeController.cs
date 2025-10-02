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
        private readonly CountryRepository _countryRepository;
        private readonly StateRepository _stateRepository;
        private readonly CityRepository _cityRepository;
        private readonly DynamicFormRepository _dynamicFormRepository;

        public HomeController(ActivityDetailRepository activityDetailRepo,ActivityRepository activityRepository, PageRepository pageRepo, 
            CountryRepository countryRepository, StateRepository stateRepository, CityRepository cityRepository,DynamicFormRepository dynamicFormRepository)
        {
            _activityDetailRepo = activityDetailRepo;
            _activityRepository = activityRepository;
            _pageRepo = pageRepo;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            _dynamicFormRepository = dynamicFormRepository;
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
                return PartialView("_DynamicForm", new List<ActivityDetail>());

            var activity = _activityRepository.GetById(activityId, "SELECTBYID");
            ViewBag.ActivityTitle = activity?.Title ?? "Dynamic Form";

            
            ViewBag.Countries = new SelectList(_countryRepository.GetAll("SELECTALL"), "ID", "CountryName");

            return PartialView("_DynamicForm", details);
        }

        [HttpGet]
        public JsonResult GetStatesByCountry(Int64 countryId)
        {
            var states = _stateRepository.GetCountryByState("SELECTBYCOUNTRY", countryId);
            var result = states.Select(s => new { id = s.ID, name = s.StateName }).ToList();
            return Json(result);
        }

        [HttpGet]
        public JsonResult GetCitiesByState(Int64 stateId)
        {
            var cities = _cityRepository.GetStateByCity("SELECTBYSTATE", stateId);
            var result = cities.Select(c => new { id = c.ID, name = c.CityName }).ToList();
            return Json(result);
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

        [HttpPost]
        public IActionResult SaveFormData([FromBody] List<DynamicFormData> submissions)
        {
            if (submissions == null || submissions.Count == 0)
            {
                return Json(new { success = false, message = "No data received" });
            }

            try
            {
                foreach (var item in submissions)
                {
                    _dynamicFormRepository.InsertOrUpdate(item, "INSERT"); 
                }

                return Json(new { success = true, message = "Data saved successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult PageMaster()
        {
            var pages = _pageRepo.GetAll("SELECTALL");
            return View(pages);
        }

        public IActionResult DynamicDataList(Int64? activityId = null)
        {
            var list = _dynamicFormRepository.GetAll("SELECT", activityId);
            return View(list);
        }

        public IActionResult Edit(Int64 id)
        {
            var data = _dynamicFormRepository.GetAll("GETBYID", id).FirstOrDefault();
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DynamicFormData model)
        {
            if (ModelState.IsValid)
            {
                _dynamicFormRepository.InsertOrUpdate(model, "UPDATE");
                return RedirectToAction("DynamicDataList");
            }
            return View(model);
        }

        public IActionResult Delete(Int64 id)
        {
            var data = _dynamicFormRepository.GetAll("GETBYID", id).FirstOrDefault();
            if (data == null)
            {
                return NotFound();
            }

            _dynamicFormRepository.Delete(id, "DELETE");
            return RedirectToAction("DynamicDataList");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        private void LoadDropdowns(long? countryId = null, long? stateId = null)
        {
            ViewBag.Countries = new SelectList(_countryRepository.GetAll("SELECTALL"), "ID", "CountryName", countryId);
            ViewBag.States = new SelectList(_stateRepository.GetAll("SELECTALL"), "ID", "StateName", stateId);
            ViewBag.Cities = new SelectList(_cityRepository.GetAll("SELECTALL"), "ID", "CityName");
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = SysActivity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
