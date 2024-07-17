

using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
	public class Student
	{
		[Column("First_name")]
		public int FirstName { get; set; }
		[Column("Last_Name")]
		public int LastName { get; set; }
		
		[Column("Email")]
		public int	Email { get; set; }
		//public int Id{ get; set; }

	}
}
