
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
	public class CourseDisplay
	{
		[Column("Name_course")]
		public string Name { get; set; }
		[Column("Perequisites")]
		public string Prerequisites { get; set; }

		[Column("Difficulty")]
		public string Difficulty { get; set; }

		[Column("ImageData")]
		public string Image { get; set; }
		
		[Column("Preview")]
		public string ShortDescription { get; set; }

		public Byte[] ImageContents { get; set; }


	}
}
