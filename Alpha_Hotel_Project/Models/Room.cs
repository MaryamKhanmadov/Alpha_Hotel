﻿using Alpha_Hotel_Project.Entities;
using Alpha_Hotel_Project.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alpha_Hotel_Project.Models
{
    public class Room : EntityModel
    {
        [StringLength(maximumLength:30)]
        public string Name { get; set; }
        [StringLength(maximumLength: 50)]
        public string Location { get; set; }
        public string Descreption { get; set; }
        [Range(0, int.MaxValue)]
        public int ViewCount { get; set; } = 0;
        [Range(0,int.MaxValue)]
        public double AdultPrice { get; set; }
        public double ChildPrice { get; set; }
        [Range(minimum:0,maximum:10)]
        public int Capacity { get; set; }
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
        public List<OrderItem>? OrderItems { get; set; }
        [NotMapped] 
        public List<Guid>? RoomImageIds { get; set; }
    }
}
