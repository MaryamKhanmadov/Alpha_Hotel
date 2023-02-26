﻿using Alpha_Hotel_Project.Entities;
using System.ComponentModel.DataAnnotations;

namespace Alpha_Hotel_Project.Models
{
    public class BlogComment : EntityModel
    {
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 5)]
        public string FullName { get; set; }
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 5), DataType(DataType.EmailAddress)]
        public string CommentEmail { get; set; }
        [Required]
        [StringLength(maximumLength: 300, MinimumLength = 10)]
        public string Comment { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime MessageTime { get; set; } = DateTime.Now;
        public Guid BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
