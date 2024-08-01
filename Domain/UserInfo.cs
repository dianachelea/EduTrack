using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
	public class UserInfo
	{
		[Column("Username")]
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
