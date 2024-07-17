using Domain;

namespace WebApiContracts.Mappers
{
    public static class UserCredentialsMapper
    {
        /* Register Mapper*/
        public static UserCredentials MapTestToDomain(this UserCredentialsContract credentials)
        {
            return new UserCredentials
            {
                Username = credentials.Username,
                Password = credentials.Password,
                Email = credentials.Email,
                Last_name = credentials.Last_name,
                First_name = credentials.First_name,
                Phone_number = credentials.Phone_number,
            };
        }

        public static UserCredentials LoginMapper(this UserCredentialsContract credentials) 
        {
            return new UserCredentials
            {
                Username = credentials.Username,
                Password = credentials.Password,
                Email = credentials.Email,
            };

        }

    }
}
