using Domain;

namespace WebApiContracts.Mappers
{
    public static class MapToAssignment
    {
        public static AssignmentDo MapTestToDomain(this AssignmentContentContract assignmentData)
        {
            return new AssignmentDo
            {
                Assignment_name = assignmentData.Assignment_name,
                Assignment_description = assignmentData.Assignment_description,
                Assignment_preview = assignmentData.Assignment_preview
            };
        }
    }
}
