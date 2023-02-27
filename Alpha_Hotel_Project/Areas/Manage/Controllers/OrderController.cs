using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Helpers;
using Alpha_Hotel_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace Alpha_Hotel_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public OrderController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index(int page=1)
        {
            var query = _appDbContext.Orders.Include(x => x.OrderItem).AsQueryable();
            PaginatedList<Order> orders = PaginatedList<Order>.Create(query, 5, page);
            return View(orders);
        }
        public IActionResult Detail(Guid id)
        {
            Order order = _appDbContext.Orders.Include(x => x.OrderItem).FirstOrDefault(x => x.Id == id);
            if (order == null) return View("Error");
            return View(order);
        }
        public IActionResult Accept(Guid id)
        {
            Order order = _appDbContext.Orders.FirstOrDefault(x => x.Id == id);
            if (order is null) return View("Error");
            order.OrderStatus = Enums.OrderStatus.Accepted;
            _appDbContext.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Reject(Guid id)
        {
            Order order = _appDbContext.Orders.FirstOrDefault(x => x.Id == id);
            if (order is null) return View("Error");
            order.OrderStatus = Enums.OrderStatus.Rejected;
            _appDbContext.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
