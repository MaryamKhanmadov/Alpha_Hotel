using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Alpha_Hotel_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BlogCategoryController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public BlogCategoryController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            List<BlogCategory> blogCategories = _appDbContext.BlogCategories.Where(x => x.IsDeleted == false).ToList();
            return View(blogCategories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BlogCategory category)
        {
            if (!ModelState.IsValid) return View();
            _appDbContext.BlogCategories.Add(category);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(Guid id)
        {
            BlogCategory category = _appDbContext.BlogCategories.FirstOrDefault(x => x.Id == id);
            if (category == null) return View("Error");
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(BlogCategory category)
        {
            if (!ModelState.IsValid) return View();
            BlogCategory existcategory = _appDbContext.BlogCategories.FirstOrDefault(x => x.Id == category.Id);
            if (existcategory == null) return View("Error");

            existcategory.Name = category.Name;

            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(Guid id)
        {
            BlogCategory category = _appDbContext.BlogCategories.FirstOrDefault(x => x.Id == id);
            if (category == null) return View("Error");
            category.IsDeleted = true;
            //_appDbContext.Professions.Remove(profession);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
