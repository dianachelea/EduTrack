using Domain;

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

    }
}
