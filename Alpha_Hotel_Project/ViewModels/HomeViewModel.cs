using Alpha_Hotel_Project.Models;

namespace Alpha_Hotel_Project.ViewModels
{
    public class HomeViewModel
    {
        public List<Staff> Staffs { get; set; }
        public List<Profession> Professions { get; set; }
        public List<Facility> Facilities { get; set; }
        public List<Slider> Sliders { get; set; }
        public List<Partner> Partners { get; set; }
        public List<About> Abouts { get; set; }
        public List<Room> Rooms { get; set; }
        public List<Category> Categories { get; set; }
    }
}
