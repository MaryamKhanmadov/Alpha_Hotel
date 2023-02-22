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
                Abouts = _appDbContext.Abouts.Where(x => x.IsDeleted == false).ToList(),
                Rooms = _appDbContext.Rooms.Include(x => x.Category).Where(x => x.IsDeleted == false).ToList(),
                RoomImages = _appDbContext.RoomImages.ToList()
            };
            return View(homeViewModel);
        }
        //[HttpGet]
        //public IActionResult BookingSystem()
        //{
        //    OrderViewModel orderViewModel = new OrderViewModel
        //    {
        //        Rooms = _appDbContext.Rooms.Include(x => x.RoomImages).Where(x => x.IsDeleted == false).Include(x => x.Category).Where(x => x.IsDeleted == false).ToList(),
        //    };
        //    //return Ok(orderViewModel);
        //    return View(orderViewModel);
        //}
        //[HttpPost]
        //public IActionResult BookingSystem(/*Guid id,*/ OrderItemViewModel orderItemVM)
        //{

        //    Room room = _appDbContext.Rooms.Include(x => x.RoomImages).Include(x => x.Category).FirstOrDefault(x => x.Id == orderItemVM.Room.Id);

        //    orderItemVM.Room = room;
        //    int Startdate = orderItemVM.StartRentDate.DayOfYear;
        //    int Enddate = orderItemVM.EndRentDate.DayOfYear;
        //    int DayCount = Enddate - Startdate;
        //    //if (!ModelState.IsValid) return View();
        //    //double? TotalPrice = DayCount * room.AdultPrice;
        //    foreach (var item in _appDbContext.Orders)
        //    {
        //        int date = item.StartRentDate.DayOfYear;
        //        int date2 = item.EndRentDate.DayOfYear;
        //        for (int i = date; i <= date2; i++)
        //        {
        //            for (int j = Startdate; j <= Enddate; j++)
        //            {
        //                if (i == j)
        //                {
        //                    ModelState.AddModelError("", "Already Reserverd");
        //                    return View(orderItemVM);
        //                }
        //            }
        //        }
        //    }

        //    if (orderItemVM.EndRentDate < orderItemVM.StartRentDate)
        //    {
        //        ModelState.AddModelError("EndRentDate", "Choice correct date");
        //        return View(orderItemVM);
        //    }
        //    if (orderItemVM.StartRentDate < DateTime.Now)
        //    {
        //        ModelState.AddModelError("StartRentDate", "Choice correct date");
        //        return View(orderItemVM);
        //    }
        //    if (orderItemVM.EndRentDate < DateTime.Now)
        //    {
        //        ModelState.AddModelError("EndRentDate", "Choice correct date");
        //        return View(orderItemVM);
        //    }
        //    if (DayCount > 30)
        //    {
        //        ModelState.AddModelError("EndRentDate", "Up to 1 month reservation allowed");
        //        return View(orderItemVM);
        //    }
        //    //Order order = null;
        //    //order = new Order
        //    //{
        //    //    EndRentDate = orderVM.EndRentDate,
        //    //    ChildCount = orderVM.ChildCount,
        //    //    AdultCount = orderVM.AdultCount,
        //    //    Address = orderVM.Address,
        //    //    StartRentDate = orderVM.StartRentDate,
        //    //    eMail = orderVM.eMail,
        //    //    Fullname = orderVM.Fullname,
        //    //    PhoneNumber = orderVM.PhoneNumber,
        //    //    //TotalPrice = TotalPrice,
        //    //    Type = orderVM.Type
        //    //};
        //    //_appDbContext.Orders.Add(order);

        //    //OrderItem orderItem = null;
        //    //orderItem = new OrderItem
        //    //{
        //    //    DayCount = DayCount,
        //    //    OneDayPrice = orderVM.AdultPrice,
        //    //    //Order = order,
        //    //    RoomId = orderVM.Room.Id,
        //    //    //TotalPrice = TotalPrice
        //    //};
        //    //_appDbContext.OrderItems.Add(orderItem);
        //    _appDbContext.SaveChanges();
        //    //return RedirectToAction("bookingsystem", "home", orderItemVM.Room.Id);
        //    return Ok(orderItemVM);
        //    //return View();






        //    //OrderViewModel orderViewModel = new OrderViewModel
        //    //{
        //    //    Rooms = _appDbContext.Rooms.Include(x => x.RoomImages).Where(x => x.IsDeleted == false).Include(x => x.Category).Where(x => x.IsDeleted == false).ToList(),
        //    //};
        //    ////return Ok(orderViewModel);
        //    //return View(orderViewModel);
        //}
    }
}