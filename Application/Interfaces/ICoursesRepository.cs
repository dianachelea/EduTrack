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
		Task<IEnumerable<Course>> GetCourse(string name);
		Task<bool> AddCourse(Course course);
		Task<bool> UpdateCourse(string name, Course course);
		Task<bool> DeleteCourse(string name);
		Task<IEnumerable<CourseDisplay>> GetAllCourses();
		Task<IEnumerable<CourseDisplay>> GetCoursesByFilter(CourseFilter filter);
		Task<IEnumerable<Course>> GetCoursesByStudentEmail(string studentEmail);
		Task<IEnumerable<Student>> GetAllStudentsEnrolled(string name);
		//putem folosi o val hardcoded pt numar pt ca ar fi la fel pe toate paginile
		Task<IEnumerable<CourseDisplay>> GetRelatedCourses(string name);






	}
}
