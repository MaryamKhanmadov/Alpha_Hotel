using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Alpha_Hotel_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public CategoryController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            List<Category> categories = _appDbContext.Categories.Where(x => x.IsDeleted == false).ToList();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid) return View();
            _appDbContext.Categories.Add(category);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(Guid id)
        {
            Category category = _appDbContext.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null) return View("Error");
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Category category)
        {
            if (!ModelState.IsValid) return View();
            Category existcategory = _appDbContext.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (existcategory == null) return View("Error");

            existcategory.Name = category.Name;

            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(Guid id)
        {
            Category category = _appDbContext.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null) return View("Error");
            category.IsDeleted = true;
            //_appDbContext.Professions.Remove(profession);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
