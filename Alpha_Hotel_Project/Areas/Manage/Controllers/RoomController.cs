using Alpha_Hotel_Project.Data;
using Alpha_Hotel_Project.Helpers;
using Alpha_Hotel_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Net.Mime;

namespace Alpha_Hotel_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class RoomController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;
        public RoomController(AppDbContext context, IWebHostEnvironment env)
        {
            _appDbContext = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _appDbContext.Rooms.Include(x => x.Category).Where(x=>x.IsDeleted==false).Include(x => x.RoomImages).AsQueryable();
            PaginatedList<Room> rooms = PaginatedList<Room>.Create(query, 5, page);
            return View(rooms);
        }
        public IActionResult Create()
        {
            ViewBag.Categories = _appDbContext.Categories.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Room room)
        {
            ViewBag.Categories = _appDbContext.Categories.ToList();
            if (!ModelState.IsValid) return View(room);
            foreach (IFormFile image in room.ImageFiles)
            {
                if (image.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFiles", "Please , upload less than 2Mb");
                    return View(room);
                }
                if (image.ContentType != "image/png" && image.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFiles", "Please , upload only image file (jpeg or png) ");
                    return View(room);
                }
                RoomImage roomImage = new RoomImage()
                {
                    Room = room,
                    ImageUrl = image.SaveFile(_env.WebRootPath, "uploads/rooms"),
                    IsPoster = false
                };
                _appDbContext.RoomImages.Add(roomImage);
            }
            if (room.PosterImageFile is null)
            {
                ModelState.AddModelError("PosterImageFile", "Required");
                return View(room);
            }
            if (room.PosterImageFile.Length > 2097152)
            {
                ModelState.AddModelError("PosterImage", "Please , upload less than 2Mb");
                return View(room);
            }
            if (room.PosterImageFile.ContentType != "image/png" && room.PosterImageFile.ContentType != "image/jpeg")
            {
                ModelState.AddModelError("PosterImage", "Please , upload only image file (jpeg or png) ");
                return View(room);
            }
            RoomImage posterImage = new RoomImage()
            {
                Room = room,
                ImageUrl = room.PosterImageFile.SaveFile(_env.WebRootPath, "uploads/rooms"),
                IsPoster = true
            };
            _appDbContext.RoomImages.Add(posterImage);
            _appDbContext.Rooms.Add(room);
            _appDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(Guid id)
        {
            ViewBag.Categories = _appDbContext.Categories.ToList();
            Room room = _appDbContext.Rooms.Include(x => x.RoomImages).Include(x => x.Category).FirstOrDefault(x => x.Id == id);
            if (room is null) return View("Error");
            return View(room);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Room room)
        {
            ViewBag.Categories = _appDbContext.Categories.ToList();
            if (!ModelState.IsValid) return View(room);
            Room existroom = _appDbContext.Rooms.Include(x => x.RoomImages).Include(x=>x.Category).FirstOrDefault(x => x.Id == room.Id);
            if (existroom is null) return View("Error");
            List<RoomImage> ImagesDelet = existroom.RoomImages.FindAll(ai => !room.RoomImageIds.Contains(ai.Id) && ai.IsPoster is false);
            foreach (var item in ImagesDelet)
            {
                string path = Path.Combine(_env.WebRootPath, "uploads/rooms", item.ImageUrl);
                System.IO.File.Delete(path);
            }
            existroom.RoomImages.RemoveAll(ai => !room.RoomImageIds.Contains(ai.Id) && ai.IsPoster is false);
            if (room.ImageFiles is not null)
            {
                foreach (IFormFile image in room.ImageFiles)
                {
                    if (image.Length > 2097152)
                    {
                        ModelState.AddModelError("ImageFiles", "Please , upload less than 2Mb");
                        return View(room);
                    }
                    if (image.ContentType != "image/png" && image.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("ImageFiles", "Please , upload only image file (jpeg or png) ");
                        return View(room);
                    }
                    RoomImage roomImages = new RoomImage()
                    {
                        RoomId = room.Id,
                        ImageUrl = image.SaveFile(_env.WebRootPath, "uploads/rooms"),
                        IsPoster = false
                    };
                    _appDbContext.RoomImages.Add(roomImages);
                }
            }
            if (room.PosterImageFile is not null)
            {
                if (room.PosterImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("PosterImage", "Please , upload less than 2Mb");
                    return View(room);
                }
                if (room.PosterImageFile.ContentType != "image/png" && room.PosterImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("PosterImage", "Please , upload only image file (jpeg or png) ");
                    return View(room);
                }
                foreach (var item in existroom.RoomImages)
                {
                    if (item.IsPoster == true)
                    {
                        string path = Path.Combine(_env.WebRootPath, "uploads/rooms", item.ImageUrl);
                        System.IO.File.Delete(path);
                        _appDbContext.Remove(item);
                    }
                }
                RoomImage PosterImage = new RoomImage()
                {
                    ImageUrl = room.PosterImageFile.SaveFile(_env.WebRootPath, "uploads/rooms"),
                    IsPoster = true,
                    RoomId = room.Id
                };
                _appDbContext.RoomImages.Add(PosterImage);
            }
            existroom.Descreption = room.Descreption;
            existroom.IsParking = room.IsParking;
            existroom.IsWifi = room.IsWifi;
            existroom.CategoryId = room.CategoryId;
            existroom.AdultPrice = room.AdultPrice;
            existroom.ChildPrice = room.ChildPrice;
            existroom.IsAvaliable = room.IsAvaliable;
            existroom.Location = room.Location;
            existroom.Capacity = room.Capacity;
            existroom.Type = room.Type;
            existroom.Name = room.Name;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(Guid id)
        {
            Room room = _appDbContext.Rooms.FirstOrDefault(x => x.Id == id);
            if (room is null) return View("Error");
            room.IsDeleted = true;
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult HardDelete(Guid id)
        {
            Room room = _appDbContext.Rooms.FirstOrDefault(x=>x.Id==id);
            if (room is null) return View("Error");
            List<Order> orders = new List<Order>();
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in _appDbContext.Orders.Where(x =>x.RoomId==id))
            {
                orders.Add(item);
            }
            foreach (var item in _appDbContext.OrderItems.Where(x => x.RoomId == id))
            {
                orderItems.Add(item);
            }
            if (orders is not null)
            {
                _appDbContext.Orders.RemoveRange(orders);
                _appDbContext.OrderItems.RemoveRange(orderItems);
            }
            foreach (RoomImage image in _appDbContext.RoomImages.Where(x => x.RoomId == id))
            {
                if (System.IO.File.Exists(Path.Combine(_env.WebRootPath, "uploads/rooms", image.ImageUrl)))
                    System.IO.File.Delete(Path.Combine(_env.WebRootPath, "uploads/rooms", image.ImageUrl));
            }
            _appDbContext.Rooms.Remove(room);
            _appDbContext.SaveChanges();
            return RedirectToAction("SoftDeleteIndex");
        }
        public IActionResult SoftDeleteIndex(int page = 1)
        {
            var query = _appDbContext.Rooms.Include(x => x.Category).Where(x => x.IsDeleted == true).Include(x => x.RoomImages).AsQueryable();
            PaginatedList<Room> rooms = PaginatedList<Room>.Create(query, 5, page);
            return View(rooms);
        }
        public IActionResult Restore(Guid id)
        {
            Room room = _appDbContext.Rooms.FirstOrDefault(x => x.Id == id);
            if (room == null) return View("Error");
            room.IsDeleted = false;
            _appDbContext.SaveChanges();
            return RedirectToAction("SoftDeleteIndex");
        }
    }
}
