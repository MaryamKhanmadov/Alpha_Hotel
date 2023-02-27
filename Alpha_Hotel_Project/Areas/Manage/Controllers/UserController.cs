using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Helpers;
using Alpha_Hotel_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alpha_Hotel_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class UserController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(AppDbContext appDbContext,RoleManager<IdentityRole> roleManager)
        {
            _appDbContext = appDbContext;
            _roleManager = roleManager;
        }
        public IActionResult Index(int page=1)
        {
            var query = _appDbContext.Users.Where(x => x.Email != null).AsQueryable();
            PaginatedList<AppUser> Users = PaginatedList<AppUser>.Create(query, 5 , page);
            return View(Users);
        }
    }
}
