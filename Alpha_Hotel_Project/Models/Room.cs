using Alpha_Hotel_Project.Entities;
using Alpha_Hotel_Project.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alpha_Hotel_Project.Models
{
    public class Room : EntityModel
    {
        public string Location { get; set; }
        public string Descreption { get; set; }
        public double AdultPrice { get; set; }
        public double ChildPrice { get; set; }
        public int Capacity { get; set; }
        public int Number { get; set; }
        public bool IsAvaliable { get; set; }
        public bool IsWifi { get; set; }
        public bool IsParking { get; set; }
        public RoomType Type { get; set; }
        public Guid? CategoryId { get; set; }
        public DateTime? RoomCreationDate { get; set; } = DateTime.Now;
        public Category? Category { get; set; }
        public List<RoomImage>? RoomImages { get; set; }
        [NotMapped]
        public IFormFile? PosterImageFile { get; set; }
        [NotMapped]
        public List<IFormFile>? ImageFiles { get; set; }
        [NotMapped] 
        public List<Guid>? RoomImageIds { get; set; }
    }
}
