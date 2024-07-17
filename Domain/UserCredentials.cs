using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class UserCredentials
    {
		[Column("Username")]
		public string Username { get; set; }
		[Column("Password")]
		public string Password { get; set; }
		[Column("Email")]
		public string Email { get; set; }
		[Column("Role")]
        public string Role { get; set; }
		[Column("First_name")]
		public string FirstName { get; set; }
		[Column("Last_Name")]
		public string LastName { get; set; }
		[Column("Phone_number")]
		public string Phone { get; set; }
	}
}
