using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<List<Attendance>> GetStudentAttendance(string courseName, string studentEmail);
        Task<bool> MakeAttendance(string courseName, string lessonTitle, List<Student> students);
        Task<List<Attendance>> GetAttendance(string courseName, string lessonTitle);
    }
}
