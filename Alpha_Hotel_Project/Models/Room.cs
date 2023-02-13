using Alpha_Hotel_Project.Entities;

namespace Alpha_Hotel_Project.Models
{
    public class Room : EntityModel
    {
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
