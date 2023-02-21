using Alpha_Hotel_Project.Enums;
using Alpha_Hotel_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Alpha_Hotel_Project.ViewModels
{
    public class OrderViewModel
    {
        public Room Room { get; set; }
        public string Fullname { get; set; }
        [Required]
        [StringLength(maximumLength: 30), DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(maximumLength: 100), DataType(DataType.EmailAddress)]
        public string? eMail { get; set; }
        [Required]
        public byte AdultCount { get; set; }
        [Required]
        public byte ChildCount { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string Address { get; set; }
        public RoomType Type { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartRentDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndRentDate { get; set; }
        public AppUser? AppUser { get; set; }
        public List<Room>? Rooms { get; set; }
        public double AdultPrice { get; set; }
    }
}
