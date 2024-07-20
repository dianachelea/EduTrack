using Domain;
using System.Xml.Linq;

namespace WebApiContracts.Mappers
{
    public static class MapperToDo
    {

        public static UserCredentials MapToUserCredentials(this LoginUserCredentialsContract credentials)
        {
            return new UserCredentials
			{
                Password = credentials.Password,
                Email = credentials.Email
            };
        }
        public static UserCredentials MapToUserRegister(this RegisterUserCredentialsContract credentials)
        {
            return new UserCredentials
			{
				Username = credentials.Username,
				FirstName = credentials.FirstName,
				LastName = credentials.LastName,
				Phone = credentials.Phone,
				Password = credentials.Password,
                Email = credentials.Email
            };
        } 
        public static Lesson MaptoLesson(this LessonContract credentials) 
        {
            return new Lesson
            {
                Name = credentials.Name,
                Description = credentials.Description,
                Assignment_name = credentials.Assignment_name,
                Assignment_description = credentials.Assignment_description,
                Assignment_file = credentials.Assignment_file,
                LessonDetails = credentials.LessonDetails,
                LessonStatus = credentials.LessonStatus,
                Assignment_preview = credentials.Assignment_preview,
                StartDate = credentials.StartDate,
                Lesson_Content = credentials.Lesson_Content

            };
        }

    }
}
