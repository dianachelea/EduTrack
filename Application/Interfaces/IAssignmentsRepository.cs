using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Interfaces
{
    public interface IAssignmentsRepository
    {
        Task<IEnumerable<AssignmentDo>> GetAssignment(string coursename, string lessontitle);
        Task<bool> AddAssignment(string coursename, string lessontitle, AssignmentDo assignmentData, string FileName);
		Task<IEnumerable<AssignmentGradeDo>> GetStudentAssignments(string coursename, string studentEmail);
        Task<bool> EditAssignment([FromQuery] string CourseName, string LessonTitle, AssignmentDo Content, string FileName);
        Task<bool> DeleteAssignment([FromQuery] string CourseName, string LessonTitle);
    }
}
