using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Models;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            List<Order> orders = _appDbContext.Orders.Include(x => x.OrderItem).ToList();
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
