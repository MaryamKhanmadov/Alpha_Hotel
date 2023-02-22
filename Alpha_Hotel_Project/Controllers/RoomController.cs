using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Models;
using Alpha_Hotel_Project.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Alpha_Hotel_Project.Controllers
{
    public class RoomController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;

        public RoomController(AppDbContext appDbContext , UserManager<AppUser> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Detail(Guid id)
        {
            Room room = _appDbContext.Rooms.Include(x => x.Category).Include(x => x.RoomImages).FirstOrDefault(x => x.Id == id);
            OrderItemViewModel orderItemViewModel = new OrderItemViewModel
            {
                Room = room,
                RoomId=room.Id,
            };
            room.ViewCount++;
            if (room is null) return View("Error");
            _appDbContext.SaveChanges();
            return View(orderItemViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> BookingSystem(Guid id, OrderItemViewModel orderItemVM)
        {
            Room room = _appDbContext.Rooms.Include(x => x.RoomImages).Include(x => x.Category).FirstOrDefault(x => x.Id == id);

            orderItemVM.Room = room;
            orderItemVM.RoomId = room.Id;
            int Startdate = orderItemVM.StartRentDate.DayOfYear;
            int Enddate = orderItemVM.EndRentDate.DayOfYear;
            int DayCount = Enddate - Startdate;
            //if (!ModelState.IsValid) return View();
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
                            return View(orderItemVM);
                        }
                    }
                }
            }

            if (orderItemVM.EndRentDate < orderItemVM.StartRentDate)
            {
                ModelState.AddModelError("EndRentDate", "Choice correct date");
                return View(orderItemVM);
            }
            if (orderItemVM.StartRentDate < DateTime.Now)
            {
                ModelState.AddModelError("StartRentDate", "Choice correct date");
                return View(orderItemVM);
            }
            if (orderItemVM.EndRentDate < DateTime.Now)
            {
                ModelState.AddModelError("EndRentDate", "Choice correct date");
                return View(orderItemVM);
            }
            if (DayCount > 30)
            {
                ModelState.AddModelError("EndRentDate", "Up to 1 month reservation allowed");
                return View(orderItemVM);
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
            AppUser member = null;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                member = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            }
            OrderViewModel orderViewModel = new OrderViewModel
            {
                OrderItemView = orderItemVM,
                Rooms = _appDbContext.Rooms.Include(x => x.RoomImages).Where(x => x.IsDeleted == false).Include(x => x.Category).ToList(),
                Room = orderItemVM.Room,
                //Room = room,
                AdultCount = orderItemVM.AdultCount,
                ChildCount = orderItemVM.ChildCount,
                Type = orderItemVM.Type,
                StartRentDate = orderItemVM.StartRentDate,
                EndRentDate = orderItemVM.EndRentDate,
                Fullname = member?.Fullname,
                eMail = member?.Email,
                PhoneNumber = member?.PhoneNumber,
            };
            return View(orderViewModel);
            _appDbContext.SaveChanges();
            //return Redirect(Url.RouteUrl(new { controller = "room", action = "bookingsystem", id = orderItemVM.Room.Id }) + "#step-2");
            //return RedirectToAction("bookingsystem", "home", orderItemVM.Room.Id , new {otherparam = });
            //return View(orderItemVM);
            //return View();
        }

        //[HttpGet]
        //public async Task<IActionResult> BookingSystem(Guid id)
        //{
        //    //return Ok(orderViewModel);
        //    AppUser member = null;

        //    if (HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        member = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        //    }
        //    OrderViewModel orderViewModel = new OrderViewModel
        //    {
        //        Rooms = _appDbContext.Rooms.Include(x => x.RoomImages).Where(x => x.IsDeleted == false).Include(x => x.Category).ToList(),
        //        Room = _appDbContext.Rooms.Include(x => x.RoomImages).Where(x => x.IsDeleted == false).Include(x => x.Category).FirstOrDefault(x => x.Id == id),
        //        //Room = room,
        //        //StartRentDate = 
        //        Fullname = member?.Fullname,
        //        eMail = member?.Email,
        //        PhoneNumber = member?.PhoneNumber,
        //    };
        //    return View(orderViewModel);
        //}
        //[HttpPost]
        //public IActionResult BookingSystem()
        //{

        //}
        [HttpPost]
        public IActionResult CheckOut(Guid id , OrderViewModel orderViewModel)
        {
            Room room = _appDbContext.Rooms.Include(x => x.RoomImages).Include(x => x.Category).FirstOrDefault(x => x.Id == id);
            orderViewModel.Room = room;
            return Ok(orderViewModel.Room);
        }





        [HttpPost]
        public IActionResult Order(OrderViewModel orderViewModel)
        {
            OrderViewModel orderViewModel1 = new OrderViewModel
            {
                Fullname = orderViewModel?.Fullname,
                Address = orderViewModel?.Address,
                City = orderViewModel?.City
            };

            return Ok(orderViewModel1);
        }
    }
}
