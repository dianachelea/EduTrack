using Application.Interfaces;
using Domain;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly IDatabaseContext _databaseContext;
        public LessonRepository(IDatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }
        public async Task<List<LessonDisplay>> GetLessons(string courseName)
        {

        }
        public async Task<Lesson> GetLesson(string lessonTitle, string teacherEmail)
        {

        }
        public async Task<bool> UpdateLesson(string lessonTitle, Lesson lesson)
        {

        }
        public async Task<bool> AddLesson(string courseTitle, string teacherEmail, Lesson lessonData)
        {

        }
        public async Task<bool> DeleteLesson(string courseName, string lessonTitle)
        {

        }
        public async Task<bool> ChangeStatus(string courseName, string lessonTitle, GCNotificationStatus status)
        {

        }
    }
}
