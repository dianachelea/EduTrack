using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AssignmentService
    {
        private readonly IGradesRepository _gradesRepository;

        public AssignmentService(IGradesRepository grades, IFileRepository fileRepository)
        {
            _gradesRepository = grades;
        }

        public async Task<bool> SolveAssignment(string courseName, string lessonTitle, string studentEmail, AssignmentSolutionDo solution,string FileName)
        {
            var solutionCheck = await this._gradesRepository.GetStudentSolution(courseName, lessonTitle, studentEmail);

            if (solutionCheck.ToList().Count != 0)
            {
                throw new Exception("Already sent solution");
                //throw new NullReferenceException("User already registered");
            }

            var addGradeResult = await this._gradesRepository.SolveAssignment(courseName, lessonTitle, studentEmail, new AssignmentSolutionDo
            {
                Solution_title = solution.Solution_title,
                Solution = solution.Solution
            },FileName);

            return addGradeResult;
        }

        public Task<IEnumerable<AssignmentSolutionDo>> GetStudentSolution(string courseName, string lessonTitle, string studentEmail)
        {
            var studentSolution = this._gradesRepository.GetStudentSolution(courseName, lessonTitle, studentEmail);

            if (studentSolution.Result == null)
            {
                throw new Exception("No solution");
            }

            return studentSolution;
        }

        public Task<IEnumerable<List<AssignmentGradeDo>>> GetAllAssignmentsSent(string courseName, string lessonTitle)
        {
            var allStudentSolution = this._gradesRepository.GetAllAssignmentsSent(courseName, lessonTitle);

            if (allStudentSolution.Result == null)
            {
                throw new Exception("No students solved this assignment");
            }

            return allStudentSolution;
        }

        public Task<IEnumerable<double>> GetGrade(string coursename, string lessontitle, string studentemail)
        {
            var gradeCheck = this._gradesRepository.GetGrade(coursename, lessontitle, studentemail);

            if (gradeCheck.Result == null)
            {
                throw new Exception("No grade");
            }

            return gradeCheck;
        }

        public async Task<bool> GradeAssignment(string coursename, string lessontitle, double grade, string studentemail)
        {

            var gradeSolution = await this._gradesRepository.GradeAssignment(coursename, lessontitle, grade,studentemail);

            return gradeSolution;
        }

        
    }
}
