
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

		//FileContentResult este specific web mvc ului asa ca putem sa salvam doar partea de bytes in loc de tot obiectul
		[Column("ImageData")]
		public Byte[] Image { get; set; }
		
		[Column("Preview")]
		public string ShortDescription { get; set; }
	}
}
