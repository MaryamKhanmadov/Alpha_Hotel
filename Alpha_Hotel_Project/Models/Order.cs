using Alpha_Hotel_Project.Entities;
using Alpha_Hotel_Project.Enums;
using System.ComponentModel.DataAnnotations;

namespace Alpha_Hotel_Project.Models
{
    public class Order : EntityModel
    {
        //[Required]
        //[StringLength(maximumLength: 70)]
        //public string Fullname { get; set; }
        //[Required]
        //[StringLength(maximumLength: 30), DataType(DataType.PhoneNumber)]
        //public string Country { get; set; }
        //public string Address { get; set; }
        //[StringLength(maximumLength: 30)]
        //public string City { get; set; }
        //[StringLength(maximumLength: 10)]
        //public string ZipCode { get; set; }
        //[StringLength(maximumLength: 100)]
        //public string? Note { get; set; }
        //public string PhoneNumber { get; set; }
        //[Required]
        //[StringLength(maximumLength: 100), DataType(DataType.EmailAddress)]
        //public string eMail { get; set; }
        //public double TotalPrice { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        //[Required]
        //public byte AdultCount { get; set; }
        //[Required]
        //public byte ChildCount { get; set; }
        //[Required]
        //[StringLength(maximumLength: 100)]
        //public RoomType Type { get; set; }

        //[Required]
        //[DataType(DataType.Date)]
        //public DateTime StartRentDate { get; set; }
        //[Required]
        //[DataType(DataType.Date)]
        //public DateTime EndRentDate { get; set; }

        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public OrderItem OrderItem { get; set; }
        public Guid? RoomId { get; set; }
        public string CardNumber { get; set; }
        public int CardMonth { get; set; }
        public int CardYear { get; set; }
        public int CVV { get; set; }
    }
}
