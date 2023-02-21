using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Models;
using Alpha_Hotel_Project.ViewModels;
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
            OrderViewModel orderViewModel = new OrderViewModel
            {
                Room = room,
            };
            room.ViewCount++;
            if (room is null) return View("Error");
            _appDbContext.SaveChanges();
            return View(orderViewModel);
        }
        [HttpPost]
        public IActionResult Detail(Guid id, OrderViewModel orderVM)
        {
            Room room = _appDbContext.Rooms.Include(x => x.RoomImages).Include(x => x.Category).FirstOrDefault(x => x.Id == id);
            
            orderVM.Room = room;
            int Startdate = orderVM.StartRentDate.DayOfYear;
            int Enddate = orderVM.EndRentDate.DayOfYear;
            int DayCount = Enddate - Startdate;
            //double? TotalPrice = DayCount * room.AdultPrice;
            foreach (var item in _appDbContext.Orders)
            {
                int date = item.StartRentDate.DayOfYear;
                int date2 = item.EndRentDate.DayOfYear;
                for (int i = date; i <= date2; i++)
                {
                    for (int j = Startdate; j <= Enddate; j++)
                    {
                        if (i == j)
                        {
                            ModelState.AddModelError("", "Already Reserverd");
                            return View(orderVM);
                        }
                    }
                }
            }

            if (orderVM.EndRentDate < orderVM.StartRentDate)
            {
                ModelState.AddModelError("EndRentDate", "Choice correct date");
                return View(orderVM);
            }
            if (orderVM.StartRentDate < DateTime.Now)
            {
                ModelState.AddModelError("StartRentDate", "Choice correct date");
                return View(orderVM);
            }
            if (orderVM.EndRentDate < DateTime.Now)
            {
                ModelState.AddModelError("EndRentDate", "Choice correct date");
                return View(orderVM);
            }
            if (DayCount > 30)
            {
                ModelState.AddModelError("EndRentDate", "Up to 1 month reservation allowed");
                return View(orderVM);
            }
            //Order order = null;
            //order = new Order
            //{
            //    EndRentDate = orderVM.EndRentDate,
            //    ChildCount = orderVM.ChildCount,
            //    AdultCount = orderVM.AdultCount,
            //    Address = orderVM.Address,
            //    StartRentDate = orderVM.StartRentDate,
            //    eMail = orderVM.eMail,
            //    Fullname = orderVM.Fullname,
            //    PhoneNumber = orderVM.PhoneNumber,
            //    //TotalPrice = TotalPrice,
            //    Type = orderVM.Type
            //};
            //_appDbContext.Orders.Add(order);

            //OrderItem orderItem = null;
            //orderItem = new OrderItem
            //{
            //    DayCount = DayCount,
            //    OneDayPrice = orderVM.AdultPrice,
            //    //Order = order,
            //    RoomId = orderVM.Room.Id,
            //    //TotalPrice = TotalPrice
            //};
            //_appDbContext.OrderItems.Add(orderItem);
            _appDbContext.SaveChanges();

            return View();
        }
    }
}
