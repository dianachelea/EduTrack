﻿using Application.Interfaces;
using Domain;
using System.Threading.Tasks;

namespace Application.Services
{
    public class LessonInventoryService
    {
        private readonly ILessonRepository _lessonRepository;

        public LessonInventoryService(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public async Task<Lesson> GetLesson(string lessonTitle, string teacherEmail)
        {
            return await _lessonRepository.GetLesson(lessonTitle, teacherEmail);
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
