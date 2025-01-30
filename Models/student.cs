using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace first.Models
{
    public class Student
    {

        [Key]
        public int ID { get; set; }




        public string Name { get; set; }


        public string Email { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

       public byte[]? ImageData { get; set; }

        // Store Thumbnail in Binary Format
        public byte[]? ThumbnailData { get; set; }


    }
}