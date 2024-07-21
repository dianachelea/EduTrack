using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContracts.Mappers
{
    public static class MapToAssignmentSolution
    {
        public static AssignmentSolutionDo MapTestToDomain(this AssignmentSolutionContract gradeData)
        {
            return new AssignmentSolutionDo
            {
                Solution_title = gradeData.Solution_title,
                Solution = gradeData.Solution ,
                FileName = gradeData.FileName
            };
        }
    }
}
