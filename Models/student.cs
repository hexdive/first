using System.ComponentModel.DataAnnotations;

namespace first.Models
{
    public class student
    {

        [Key]
        public int ID { get; set; }




        public string Name { get; set; }


        public string Email { get; set; }

    }
}
