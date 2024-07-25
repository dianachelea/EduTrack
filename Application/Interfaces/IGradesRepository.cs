using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Interfaces
{
    public interface IGradesRepository
    {
        Task<bool> SolveAssignment(string courseName, string lessonTitle, string studentEmail, AssignmentSolutionDo solution, string FileName);
        Task<IEnumerable<AssignmentSolutionDo>> GetStudentSolution(string coursename, string lessontitle, string studentemail);
        Task<IEnumerable<List<AssignmentGradeDo>>> GetAllAssignmentsSent(string coursename, string lessontitle);
        Task<bool> GradeAssignment(string coursename, string lessontitle, double grade, string studentemail);
        Task<IEnumerable<double>> GetGrade(string coursename, string lessontitle, string studentemail);

    }
}
