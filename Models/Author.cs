using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Apetrei_Alexandru_Lab2.Models
{
    public class Author
    {
        public int ID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string FullName
        {

            get
            {
                return FirstName + " " + LastName;
            }
        }

        // Navigation property – un autor are mai multe cărți
        public ICollection<Book>? Books { get; set; }
    }
}
