﻿using Application.Interfaces;
using Domain;
using System;
using System.Collections.Frozen;
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

		public async Task<bool> AddCourse(string email, Course course) //works
		{
			var teacherCourses = await this._courseRepository.GetTeacherCourses(email);

			
			if (teacherCourses.ToList().Contains(course.Name))
			{
				throw new Exception("Course already exists");
			}
			
			course.setTeacherEmail(email);

			var addingResult = await this._courseRepository.AddCourse(course);

			return addingResult;
        }

		public async Task<bool> DeleteCourse(string email, string name)  //works
		{
			
			var teacherCourses = await this._courseRepository.GetTeacherCourses(email);
			if (!teacherCourses.ToList().Contains(name))
			{
				throw new Exception("Course does not exist");
			}

			var deleteResult = await this._courseRepository.DeleteCourse(email, name);

			return deleteResult;

		}

		public List<CourseDisplay> GetAllCourses(CourseFilter filter) //works
		{
			IEnumerable<CourseDisplay> courseResult;

			if (filter.isEmpty())
			{
				courseResult = this._courseRepository.GetAllCourses();
               
            }
			else
			{
				courseResult = this._courseRepository.GetCoursesByFilter(filter);
				
			}
				
			return courseResult.ToList();
		}
		public CourseFilter GetFilters(CourseFilter filter) //works
		{
			return this._courseRepository.GetFilters(filter);
		}

		public async Task<bool> UpdateCourse(string email, string name, Course course) //works
		{
			
			var teacherCourses = await this._courseRepository.GetTeacherCourses(email);
			if (!teacherCourses.ToList().Contains(name))
			{
				return false;
			}



			var updateResult = await this._courseRepository.UpdateCourse(email,name, course);

			return updateResult;
		}
		
		public  List<CourseDisplay> GetStudentCourses(string studentEmail)  //works
		{
			var courses = this._courseRepository.GetCoursesByStudentEmail(studentEmail);

			return courses.ToList();
		}		
		public bool IsStudentEnrolledIntroCourse(string studentEmail, string courseName)  //works
		{
			var result = this._courseRepository.IsStudentEnrolledIntroCourse(studentEmail, courseName);

			return result;
		}
		public bool IsUserCourseOwner(string email, string courseName)  //works
		{
			var result = this._courseRepository.IsUserCourseOwner(email, courseName);

			return result;
		}

		public List<CourseDisplay> GetRelatedCourses(string name) //works
		{
		
			var courses = this._courseRepository.GetRelatedCourses(name);
			
			return courses.ToList();

		}

		public List<CourseDisplay> GetMostPopularCourses() //works
		{
			var courses = this._courseRepository.GetCoursesWithMaxEnrollment(2);
			
			return courses.ToList();
		}
	}
}
