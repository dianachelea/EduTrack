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

		public Course GetCourse(string name) //works
		{
			var course = this._courseRepository.GetCourse(name);

			return course.ToList().First();

		}

		public CourseInfoPage GetCoursePresentation(string name) //works
		{
			var course = this._courseRepository.GetCourseForPage(name).ToList().First();
			
			

			return course;

		}

		public List<Student> GetAllStudentsEnrolled(string courseName , string teacherEmail) //works
		{
			var students =  this._courseRepository.GetStudentsEnrolledInCourse(courseName, teacherEmail);

			return students.ToList();
		}

		public List<Lesson> GetCourseLessons(string courseName, string teacherEmail)
		{
			var lessons = this._courseRepository.GetCourseLessons(courseName, teacherEmail);

			return lessons.ToList();
		}

		public async Task<bool> EnrollToCourse(string courseName,string studentEmail) //works
		{
			var result = await this._courseRepository.EnrollStudent(courseName, studentEmail);
			
			if(result == 1)
				return true;
			return false;

		}

		//att per course of a student (all lessons of a course)
		public List<Attendance> GetStudentAttendance(string courseName, string studentEmail)        //works
		{
			var attendance = this._courseRepository.GetStudentAttendance(courseName, studentEmail);

			return attendance.ToList();
		}

		

	}
}
