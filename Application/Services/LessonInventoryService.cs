﻿using Application.Interfaces;
using Domain;
using System.Threading.Tasks;

namespace Application.Services
{
    public class LessonInventoryService
    {
        private readonly ILessonRepository _lessonRepository;

		enum LessonStatus
		{
			not_started,
			wait_for_start,
			in_progress,
			finished
		}

		public LessonInventoryService(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public async Task<IEnumerable<LessonDisplay>> GetAllLessons(string courseName)
        {
            return await _lessonRepository.GetLessons(courseName);
        } 

        public async Task<Lesson> GetLesson(string courseName, string lessonTitle)
        {
            var lesson = await _lessonRepository.GetLesson(courseName, lessonTitle);
            if(lesson.LessonStatus == LessonStatus.not_started.ToString() 
                && lesson.StartDate.DayOfYear ==  DateTime.Now.DayOfYear)
                lesson.LessonStatus = LessonStatus.wait_for_start.ToString();

			return lesson;
        }

        public async Task<bool> UpdateLesson(string lessonTitle, Lesson lessonDo)
        {
            return await _lessonRepository.UpdateLesson(lessonTitle, lessonDo);
        }

        public async Task<bool> AddLesson(string courseTitle, string teacherEmail, Lesson lessonData)
        {
            return await _lessonRepository.AddLesson(courseTitle, teacherEmail, lessonData);
        }

        public async Task<bool> DeleteLesson(string courseName, string lessonTitle)
        {
            return await _lessonRepository.DeleteLesson(courseName, lessonTitle);
        }
    }
}
