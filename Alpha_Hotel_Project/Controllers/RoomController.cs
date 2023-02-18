using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alpha_Hotel_Project.Controllers
{
    public class RoomController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public RoomController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Detail(Guid id)
        {
            Room room = _appDbContext.Rooms.Include(x => x.Category).Include(x => x.RoomImages).FirstOrDefault(x => x.Id == id);
            if (room is null) return View("Error");
            _appDbContext.SaveChanges();
            return View(room);
        }
    }
}
