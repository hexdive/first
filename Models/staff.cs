using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace first.Models
{
    public class Staff
    {

        [Key]
        public int ID { get; set; }

       


        public string Name { get; set; }


        public string Email { get; set; }


    }
}