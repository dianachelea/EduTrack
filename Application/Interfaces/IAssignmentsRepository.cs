using Domain;

namespace Application.Interfaces
{
    public interface IAssignmentsRepository
    {
        Task<IEnumerable<AssignmentDo>> GetAssignment(string coursename, string lessontitle);
        Task<bool> AddAssignment(string coursename, string lessontitle, AssignmentDo assignmentData, string FileName);
        Task<IEnumerable<List<AssignmentDo>>> GetStudentAssignments(string coursename, string studentEmail);
    }
}
