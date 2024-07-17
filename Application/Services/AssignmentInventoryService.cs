using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
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


        public async Task<bool> AddAssignment(string coursename, string lessontitle, AssignmentDo assignmentData)
        {
            var assignmentCheck = await this._assignmentsRepository.GetAssignment(coursename, lessontitle);

            

            var addAssignmentResult = await this._assignmentsRepository.AddAssignment(coursename, lessontitle, new AssignmentDo
            {
                Assignment_name = assignmentData.Assignment_name,
                Assignment_description = assignmentData.Assignment_description,
                Assignment_preview = assignmentData.Assignment_preview,
                Assignment_file = assignmentData.Assignment_file
            });

            return addAssignmentResult;
        }

    }
}
