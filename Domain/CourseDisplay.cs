using Domain.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
	public class CourseDisplay
	{
		public string Name { get; set; }
		public string Prerequisites { get; set; }
		
		//enumul este definit in domain in enums
		public CourseDifficulty Difficulty { get; set; }
		
		//FileContentResult este specific web mvc ului asa ca putem sa salvam doar partea de bytes in loc de tot obiectul
		public Byte[] Image { get; set; }
	}
}
