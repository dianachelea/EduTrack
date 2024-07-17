using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class CourseService
	{
		private readonly ICoursesRepository _courseRepository;
		public CourseService(ICoursesRepository courseRepository) {
			_courseRepository = courseRepository;
		}

		public async Task<Course> GetCourse(string name)
		{
			var course = await this._courseRepository.GetCourse(name);

			return course.ToList().First();

		}

		public async Task<List<Student>> GetAllStudentsEnrolled(string name , string teacherEmail)
		{
			var students = await this._courseRepository.GetStudentsEnrolledInCourse(name, teacherEmail);

			return students.ToList();
		}

		


	}
}
