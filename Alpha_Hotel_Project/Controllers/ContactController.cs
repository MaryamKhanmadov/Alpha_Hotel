using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Alpha_Hotel_Project.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public ContactController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(ContactMessage message)
        {
            if (!ModelState.IsValid) return View(message);
            _appDbContext.ContactMessages.Add(message);
            _appDbContext.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
