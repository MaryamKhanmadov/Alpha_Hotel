using System.ComponentModel.DataAnnotations.Schema;

namespace Alpha_Hotel_Project.Models
{
    public class Setting
    {
        public int Id { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
