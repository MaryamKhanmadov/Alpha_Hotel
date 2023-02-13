using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alpha_Hotel_Project.Controllers
{
    public class StaffController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public StaffController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            List<Staff> staffs = _appDbContext.Staffs.Include(x => x.Profession).Where(x => x.IsDeleted == false).ToList();
            return View(staffs);
        }
    }
}
