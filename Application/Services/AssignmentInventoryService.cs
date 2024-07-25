using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AssignmentInventoryService
    {
        private readonly IAssignmentsRepository _assignmentsRepository;

        public AssignmentInventoryService(IAssignmentsRepository assignments)
        {
            _assignmentsRepository = assignments;
        }

        public Task<IEnumerable<AssignmentDo>> GetAssignment(string coursename, string lessontitle)
        {
            var allAssignments = this._assignmentsRepository.GetAssignment(coursename, lessontitle);

            if (allAssignments.Result == null)
            {
                throw new Exception("No assignments");
            }

            return allAssignments;
        }


        public async Task<bool> AddAssignment(string coursename, string lessontitle, AssignmentDo assignmentData, string FileName)
        {
            var assignmentCheck = await this._assignmentsRepository.GetAssignment(coursename, lessontitle);

            var addAssignmentResult = await this._assignmentsRepository.AddAssignment(coursename, lessontitle, new AssignmentDo
            {
                Assignment_name = assignmentData.Assignment_name,
                Assignment_description = assignmentData.Assignment_description,
                Assignment_preview = assignmentData.Assignment_preview
            }, FileName);

            return addAssignmentResult;
        }

        public Task<IEnumerable<List<AssignmentDo>>> GetStudentAssignments(string coursename, string studentEmail)
        {
            var allAssignments = this._assignmentsRepository.GetStudentAssignments(coursename, studentEmail);

            if (allAssignments.Result == null)
            {
                throw new Exception("No students solved this assignment");
            }

            return allAssignments;
        }

        public async Task<ActionResult<bool>> EditAssignment([FromQuery] string courseName, string lessonTitle, AssignmentDo content, string FileName)
        {
            var assignmentCheck = await this._assignmentsRepository.GetAssignment(courseName, lessonTitle);

            var assignmentEdited = await this._assignmentsRepository.EditAssignment(courseName, lessonTitle, new AssignmentDo
            {
                Assignment_name = content.Assignment_name,
                Assignment_description = content.Assignment_description,
                Assignment_preview = content.Assignment_preview
            },FileName);

            return assignmentEdited;
        }

        public async Task<ActionResult<bool>> DeleteAssignment([FromQuery] string CourseName, string LessonTitle)
        {
            var assignmentCheck = await this._assignmentsRepository.GetAssignment(CourseName, LessonTitle);

            var assignmentDeleted = await this._assignmentsRepository.DeleteAssignment(CourseName, LessonTitle);

            return assignmentDeleted;
        }

    }
}
