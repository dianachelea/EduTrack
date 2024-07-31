using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
	 public interface ICoursesRepository
	 {
		IEnumerable<Course> GetCourse(string name);
		Task<bool> AddCourse( Course course);
		Task<bool> UpdateCourse(string email, string name, Course course);
		Task<bool> DeleteCourse(string email, string name);
		IEnumerable<CourseDisplay> GetAllCourses();
		CourseFilter GetFilters(CourseFilter filter);
		IEnumerable<CourseDisplay> GetCoursesByFilter(CourseFilter filter);
		IEnumerable<CourseDisplay> GetCoursesByStudentEmail(string studentEmail);
	
		IEnumerable<CourseDisplay> GetRelatedCourses(string name);
		Task<IEnumerable<string>> GetTeacherCourses(string email);
		IEnumerable<Student> GetStudentsEnrolledInCourse(string name, string teacherEmail);
		IEnumerable<Lesson> GetCourseLessons(string name, string teacherEmail);

		IEnumerable<CourseDisplay> GetCoursesWithMaxEnrollment(int numberOfCourses);
		Task<int> EnrollStudent(string courseName, string studentEmail);
		
		IEnumerable<Attendance>  GetStudentAttendance(string courseName, string studentEmail);
		IEnumerable<CourseInfoPage> GetCourseForPage(string name);
		bool IsStudentEnrolledIntroCourse(string studentEmail, string courseName);
		//IEnumerable<CourseDisplay> GetSortedCourses(string order);
	}
}
