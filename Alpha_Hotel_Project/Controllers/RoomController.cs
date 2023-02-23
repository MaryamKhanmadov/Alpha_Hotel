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
        public async Task<IActionResult> Detail(Guid id)
        {
            AppUser member = null;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                member = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            }
            Room room = _appDbContext.Rooms.Include(x => x.Category).Include(x => x.RoomImages).FirstOrDefault(x => x.Id == id);
            OrderViewModel orderViewModel = new OrderViewModel
            {
                Room = room,
                Fullname = member?.Fullname,
                PhoneNumber = member?.PhoneNumber,
                eMail = member?.Email
            };
            room.ViewCount++;
            if (room is null) return View("Error");
            _appDbContext.SaveChanges();
            return View(orderViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> BookingSystem(Guid id, OrderViewModel orderVM)
        {
            Room room = _appDbContext.Rooms.Include(x => x.RoomImages).Include(x => x.Category).FirstOrDefault(x => x.Id == id);

            orderVM.Room = room;
            if (!ModelState.IsValid) { return View(orderVM); }

            int Startdate = orderVM.StartRentDate.DayOfYear;
            int Enddate = orderVM.EndRentDate.DayOfYear;
            int DayCount = Enddate - Startdate;
            //if (!ModelState.IsValid) return View();
            double TotalPrice = DayCount * (room.AdultPrice*orderVM.AdultCount + room.ChildPrice*orderVM.ChildCount);
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
            if (orderVM.ChildCount < 0 || orderVM.AdultCount < 0)
            {
                ModelState.AddModelError("ChildCount", "Select correct count");
                return View(orderVM);
            }
            if (orderVM.StartRentDate.DayOfYear == orderVM.EndRentDate.DayOfYear)
            {
                ModelState.AddModelError("StartRentDate", "Reservation allowed from 1 up to 30 days");
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
            AppUser member = null; 

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                member = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            }
            OrderViewModel orderViewModel = new OrderViewModel
            {
                Rooms = _appDbContext.Rooms.Include(x => x.RoomImages).Where(x => x.IsDeleted == false).Include(x => x.Category).ToList(),
                Room = orderVM.Room,
                //Room = room,
                AdultCount = orderVM.AdultCount,
                ChildCount = orderVM.ChildCount,
                Type = orderVM.Type,
                StartRentDate = orderVM.StartRentDate,
                EndRentDate = orderVM.EndRentDate,
                ZipCode = orderVM.ZipCode,
                Note = orderVM.Note,
                Country = orderVM.Country,
                City = orderVM.City,
                Address = orderVM.Address,
                Fullname = member?.Fullname,
                eMail = member?.Email,
                PhoneNumber = member?.PhoneNumber,
                AppUserId = orderVM.AppUserId,
            };
            return Ok(orderViewModel);
            _appDbContext.SaveChanges();
            //return Redirect(Url.RouteUrl(new { controller = "room", action = "bookingsystem", id = orderItemVM.Room.Id }) + "#step-2");
            //return RedirectToAction("bookingsystem", "home", orderItemVM.Room.Id , new {otherparam = });
            //return View(orderItemVM);
            //return View();
        }

        [HttpPost]
        public async Task<IActionResult> Order(Guid id,OrderViewModel orderViewModel)
        {
            Room room = _appDbContext.Rooms.FirstOrDefault(x=>x.Id==id);
            OrderViewModel orderVM = new OrderViewModel
            {
                RoomId = room.Id,
                Room = room,
                CardMonth = orderViewModel.CardMonth,
                CardNumber = orderViewModel.CardNumber,
                CardYear = orderViewModel.CardYear,
                CVV = orderViewModel.CVV,
            };
            return Ok(orderViewModel);
        }

        //[HttpPost]
        //public async Task<IActionResult> Detail(Guid id, OrderItemViewModel orderItemVM)
        //{
        //    Room room = _appDbContext.Rooms.Include(x => x.RoomImages).Include(x => x.Category).FirstOrDefault(x => x.Id == id);

        //    orderItemVM.Room = room;
        //    orderItemVM.RoomId = room.Id;
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
        //    AppUser member = null;

        //    if (HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        member = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        //    }
        //    OrderViewModel orderViewModel = new OrderViewModel
        //    {
        //        OrderItemView = orderItemVM,
        //        Rooms = _appDbContext.Rooms.Include(x => x.RoomImages).Where(x => x.IsDeleted == false).Include(x => x.Category).ToList(),
        //        Room = orderItemVM.Room,
        //        //Room = room,
        //        AdultCount = orderItemVM.AdultCount,
        //        ChildCount = orderItemVM.ChildCount,
        //        Type = orderItemVM.Type,
        //        StartRentDate = orderItemVM.StartRentDate,
        //        EndRentDate = orderItemVM.EndRentDate,
        //        Fullname = member?.Fullname,
        //        eMail = member?.Email,
        //        PhoneNumber = member?.PhoneNumber,
        //    };
        //    return View(orderViewModel);
        //    _appDbContext.SaveChanges();
        //    //return Redirect(Url.RouteUrl(new { controller = "room", action = "paymentinfo", orderItemVM }));
        //    //return RedirectToAction("bookingsystem", "home", orderItemVM.Room.Id , new {otherparam = });
        //    //return Ok(orderViewModel);
        //    //return View();
        //}

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
            return View(orderViewModel);
        }


        [HttpPost]
        public IActionResult paymentinfo(OrderViewModel orderViewModel)
        {
            OrderViewModel orderView = new OrderViewModel
            {
                
            };
            //OrderViewModel orderViewModel1 = new OrderViewModel
            //{
            //    Fullname = orderViewModel?.Fullname,
            //    Address = orderViewModel?.Address,
            //    City = orderViewModel?.City,
            //    OrderItemView = orderViewModel.OrderItemView,
            //};

            return Ok(orderViewModel);
        }
        
    }
}
