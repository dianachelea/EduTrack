using Domain;

namespace Application.Interfaces
{
    public interface IAssignmentsRepository
    {
        Task<IEnumerable<AssignmentDo>> GetAssignment(string coursename, string lessontitle);
        Task<bool> AddAssignment(string coursename, string lessontitle, AssignmentDo assignmentData);
    }
}
