using Alpha_Hotel_Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Alpha_Hotel_Project.Data
{
    public class AppDbContext:IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
        }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
    }
}
