using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Models;
using Alpha_Hotel_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Alpha_Hotel_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel
            {
                Staffs = _appDbContext.Staffs.Include(x => x.Profession).Where(x => x.IsDeleted == false).ToList(),
                Facilities = _appDbContext.Facilities.Where(x => x.IsDeleted == false).ToList(),
                Sliders = _appDbContext.Sliders.Where(x => x.IsDeleted == false).ToList(),
                Partners = _appDbContext.Partners.Where(x => x.IsDeleted == false).ToList(),
                Abouts = _appDbContext.Abouts.Where(x => x.IsDeleted == false).ToList()
            };
            return View(homeViewModel);
        }
    }
}