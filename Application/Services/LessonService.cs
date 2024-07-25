using Application.Interfaces;
using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class LessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IAttendanceRepository _attendanceRepository; 
        private readonly LessonInventoryService _lessonInventory; 

        public LessonService(ILessonRepository lessonRepository, IAttendanceRepository attendanceRepository, LessonInventoryService lessonInventory)
        {
            _lessonRepository = lessonRepository;
            _attendanceRepository = attendanceRepository;
			_lessonInventory = lessonInventory;
		}

        public async Task<bool> ChangeStatus(string courseName, string lessonTitle, string status)
        {
            return await _lessonRepository.ChangeStatus(courseName, lessonTitle, status);
        }

        public async Task<bool> MakeAttendance(string courseName, string lessonTitle, List<Student> students)
        {
            return await _attendanceRepository.MakeAttendance(courseName, lessonTitle, students);
        }

        public async Task<List<Attendance>> GetAttendance(string courseName, string lessonTitle)
        {
			return await _attendanceRepository.GetAttendance(courseName, lessonTitle);
        }
        public async Task<List<Attendance>> GetSAttendance(string courseName, string email)
        {
			return await _attendanceRepository.GetStudentAttendance(courseName, email);
        }


    }
}
