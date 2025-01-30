using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace first.Models
{
    public class Patient
    {

        [Key]
        public int ID { get; set; }



        [Required]
        [StringLength(100)]
        public string Name { get; set; }



        [Range(18, 70)] 
        public int Age { get; set; }

        [Required]
        [EmailAddress] 
        public string Email { get; set; }

        [Required]
        public DateTime AttendingDate { get; set; } = DateTime.UtcNow;
    }
}


