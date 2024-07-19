﻿using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ILessonRepository
    {
        Task<List<LessonDisplay>> GetLessons(string courseName);
        Task<Lesson> GetLesson(string lessonTitle, string teacherEmail);
        Task<bool> UpdateLesson(string lessonTitle, Lesson lesson);
        Task<bool> AddLesson(string courseTitle, string teacherEmail, Lesson lessonData);
        Task<bool> DeleteLesson(string courseName, string lessonTitle);
        Task<bool> ChangeStatus(string courseName, string lessonTitle, GCNotificationStatus status);

    }
}
