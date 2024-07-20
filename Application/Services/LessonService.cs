﻿using Application.Interfaces;
using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class LessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IAttendanceRepository _attendanceRepository; 

        public LessonService(ILessonRepository lessonRepository, IAttendanceRepository attendanceRepository)
        {
            _lessonRepository = lessonRepository;
            _attendanceRepository = attendanceRepository;
        }

        public async Task<bool> ChangeStatus(string courseName, string lessonTitle, string status)
        {
            return await _lessonRepository.ChangeStatus(courseName, lessonTitle, status);
        }

        public async Task<bool> MakeAttendance(string courseName, string lessonTitle, List<User> users)
        {
            return await _attendanceRepository.MakeAttendance(courseName, lessonTitle, users);
        }

        public async Task<List<Attendance>> GetAllAttendance(string courseName, string teacherEmail)
        {
            return await _attendanceRepository.GetAllAttendance(courseName, teacherEmail);
        }
    }
}
