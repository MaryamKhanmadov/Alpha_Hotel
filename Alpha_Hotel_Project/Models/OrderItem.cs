using Alpha_Hotel_Project.Entities;

namespace Alpha_Hotel_Project.Models
{
    public class OrderItem : EntityModel
    {
        public double OneDayPrice { get; set; }
        public string RoomName { get; set; }
        public int DayCount { get; set; }
        public double TotalPrice { get; set; }
        public Guid RoomId { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public Room Room { get; set; }
    }
}
