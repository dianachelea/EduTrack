using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class CourseInventoryService
	{
		private readonly ICoursesRepository _courseRepository;

		public CourseInventoryService(ICoursesRepository courseRepository)
		{
			_courseRepository = courseRepository;
		}

		public async Task<bool> AddCourse(string email, Course course)
		{
			var teacherCourses = await this._courseRepository.GetTeacherCourses(email);

			//var courseCheck = await this._courseRepository.GetCourse(course.Name);
			//courseCheck.ToList().Count != 0 ||
			if (teacherCourses.ToList().Contains(course.Name))
			{
				throw new Exception("Course already exists");
			}

			var addingResult = await this._courseRepository.AddCourse(email, course);

			return addingResult;
        }

		public async Task<bool> DeleteCourse(string email, string name)
		{
			//var courseCheck = await this._courseRepository.GetCourse(name);
			var teacherCourses = await this._courseRepository.GetTeacherCourses(email);
			if (!teacherCourses.ToList().Contains(name))
			{
				throw new Exception("Course does not exist");
			}

			var deleteResult = await this._courseRepository.DeleteCourse(email, name);

			return deleteResult;

		}

		public async Task<List<CourseDisplay>> GetAllCourses(CourseFilter filter)
		{
			IEnumerable<CourseDisplay> courseResult;

			if (filter.isEmpty())
			{
				 courseResult = await this._courseRepository.GetAllCourses();
			}

			else
			{
				courseResult = await this._courseRepository.GetCoursesByFilter(filter);
			}
				
			return courseResult.ToList();
		}

		public async Task<bool> UpdateCourse(string email, string name, Course course)
		{
			//var courseCheck = await this._courseRepository.GetCourse(name);
			var teacherCourses = await this._courseRepository.GetTeacherCourses(email);
			if (!teacherCourses.ToList().Contains(name))
			{
				throw new Exception("Course does not exist");
			}

			var updateResult = await this._courseRepository.UpdateCourse(email,name, course);

			return updateResult;
		}
		/*
		public async Task<List<CourseDisplay>> GetCoursesByFilter(CourseFilter filter)
		{
			var filteredCourses = await this._courseRepository.GetCoursesByFilter(filter);

			return filteredCourses.ToList();

		}
		*/
		public async Task<List<Course>> GetStudentCourses(string studentEmail)
		{
			//assume string is verified in frontend?

			var courses = await this._courseRepository.GetCoursesByStudentEmail(studentEmail);

			return courses.ToList();

		}

		public async Task<List<CourseDisplay>> GetRelatedCourses(string name)
		{
			//throw new Exception("tbd");
			var courses = await this._courseRepository.GetRelatedCourses(name);
			return courses.ToList();

		}
	}
}
