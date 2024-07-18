﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
	public class Course 
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
		
		[Column("Description")]
		public string Description { get; set; }
		
		[Column("Preview")]
		public string ShortDescription { get; set; }

		[Column("Category")]
		public string Category { get; set; }

		[Column("Learning_topics")]
		public List<string> LearningTopics { get; set; }
		public int StudentsEnrolled { get; set; } = 0;
		
		[Column("Time")]
		public string Duration { get; set; }

		[Column("TeacherEmail")]
		public string TeacherEmail	{ get; set; }
	}
}
