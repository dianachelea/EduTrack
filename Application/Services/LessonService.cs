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
		enum LessonStatus
		{
			not_started,
			wait_for_start,
			in_progress,
			finished
		}

		public LessonService(ILessonRepository lessonRepository, IAttendanceRepository attendanceRepository, LessonInventoryService lessonInventory)
        {
            _lessonRepository = lessonRepository;
            _attendanceRepository = attendanceRepository;
			_lessonInventory = lessonInventory;
		}

		public async Task<bool> ChangeLessonStatus(string courseName, string lessonTitle, string newStatus, string teacherEmail)
		{
			if(LessonStatus.finished.ToString() != newStatus
				&& LessonStatus.in_progress.ToString() != newStatus
				&& LessonStatus.not_started.ToString() != newStatus
				&& LessonStatus.wait_for_start.ToString() != newStatus)
				return false;

			var lesson = await _lessonRepository.GetLesson(courseName, lessonTitle);
			if ((lesson.LessonStatus == LessonStatus.not_started.ToString() && newStatus == LessonStatus.in_progress.ToString())
				&& lesson.StartDate.DayOfYear == DateTime.Now.DayOfYear)
				return await _lessonRepository.ChangeStatus(courseName, lessonTitle, newStatus, teacherEmail);
			if ((lesson.LessonStatus == LessonStatus.in_progress.ToString()) && newStatus == LessonStatus.finished.ToString())
				return await _lessonRepository.ChangeStatus(courseName, lessonTitle, newStatus, teacherEmail);

			return false;
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
