using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Models;
using Alpha_Hotel_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Alpha_Hotel_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class FacilityController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public FacilityController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel
            {
                Facilities = _appDbContext.Facilities.ToList()
            };
            return View(homeViewModel);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Facility facility)
        {
            if (!ModelState.IsValid) return View();
            _appDbContext.Facilities.Add(facility);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(Guid id)
        {
            Facility facility = _appDbContext.Facilities.FirstOrDefault(x => x.Id == id);
            if (facility == null) return View("Error");
            return View(facility);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Facility facility)
        {
            if (!ModelState.IsValid) return View();
            Facility existfacility = _appDbContext.Facilities.FirstOrDefault(x => x.Id == facility.Id);
            if (existfacility == null) return View("Error");

            existfacility.Title = facility.Title;
            existfacility.Descreption = facility.Descreption;
            existfacility.Icon = facility.Icon;

            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(Guid id)
        {
            Facility facility = _appDbContext.Facilities.FirstOrDefault(x => x.Id == id);
            if (facility == null) return View("Error");
            facility.IsDeleted = true;
            //_appDbContext.Professions.Remove(profession);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
