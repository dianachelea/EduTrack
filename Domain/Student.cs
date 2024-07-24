

using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Student
    {
        [Column("First_name")]
        public string FirstName { get; set; }
        [Column("Last_Name")]
        public string LastName { get; set; }

        [Column("Email")]
        public string Email { get; set; }
        //public int Id{ get; set; }

    }
}