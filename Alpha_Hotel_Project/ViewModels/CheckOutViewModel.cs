using Alpha_Hotel_Project.Models;

namespace Alpha_Hotel_Project.ViewModels
{
    public class CheckOutViewModel
    {
        public Room Room { get; set; }
        public Guid RoomId { get; set; }
        public Order? Order { get; set; }
        public Guid? OrderId { get; set; }
        public string CardNumber { get; set; }
        public int CardMonth { get; set; }
        public int CardYear { get; set; }
        public int CVV { get; set; }
    }
}
