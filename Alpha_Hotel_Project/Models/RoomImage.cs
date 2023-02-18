using Alpha_Hotel_Project.Entities;

namespace Alpha_Hotel_Project.Models
{
    public class RoomImage:EntityModel
    {
        public Guid? RoomId { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsPoster { get; set; }
        public Room? Room { get; set; }
    }
}
