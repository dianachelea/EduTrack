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
		Task<Course> GetCourse(string name);
		Task<bool> AddCourse(Course course);
		Task<bool> UpdateCourse(string name, Course course);
		Task<bool> DeleteCourse(string name);
		Task<List<CourseDisplay>> GetAllCourses();
		Task<List<CourseDisplay>> GetCoursesByFilter(CourseFilter filter);
		Task<List<Course>> GetCoursesByStudentEmail(string studentEmail);
		Task<Student> GetAllStudentsEnrolled(string name);
		//putem folosi o val hardcoded pt numar pt ca ar fi la fel pe toate paginile
		Task<List<CourseDisplay>> GetRelatedCourses(string name);






	}
}
