using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class UserService
	{
		private readonly IPasswordHasher _passwordHasher;
		private readonly IAuthenticationRepository _authenticationRepository;
		private readonly IIdentityHandler _identityHandler;

		public UserService(IPasswordHasher passwordHasher,
			IAuthenticationRepository authenticationRepository,
			IIdentityHandler identityHandler)
		{
			_passwordHasher = passwordHasher;
			_identityHandler = identityHandler;
			_authenticationRepository = authenticationRepository;
		}

		public async Task<bool> RegisterUser(UserCredentials credentials)
		{
			var hashedPassword = await RegisterPasswordHasher(credentials);
			var registerResult = await this._authenticationRepository.RegisterUser(new UserCredentials
			{
				Username = credentials.Username,
				Password = hashedPassword,
				Email = credentials.Email,
				Role = "user",
				Phone = credentials.Phone
			});

			return registerResult;
		}
		public async Task<bool> RegisterTeacher(UserCredentials credentials)
		{
			var hashedPassword = await RegisterPasswordHasher(credentials);
			var registerResult = await this._authenticationRepository.RegisterUser(new UserCredentials
			{
				Username = credentials.Username,
				Password = hashedPassword,
				Email = credentials.Email,
				Role = "teacher",
				Phone = credentials.Phone
			});

			return registerResult;
		}

		//DEV
		public async Task<bool> RegisterAdmin(UserCredentials credentials)
		{
			var hashedPassword = await RegisterPasswordHasher(credentials);
			var registerResult = await this._authenticationRepository.RegisterUser(new UserCredentials
			{
				Username = credentials.Username,
				Password = hashedPassword,
				Email = credentials.Email,
				Role = "admin",
				Phone = credentials.Phone
			});

			return registerResult;
		}

		/// <summary>
		/// Verify if there is any user with the same email registered
		/// If so - throw an exception
		/// Else - hash the password and return it
		/// </summary>
		/// <param name="credentials">The user credentials</param>
		/// <returns>The hashed password</returns>
		/// <exception cref="Exception">If the user is already registred</exception>
		private async Task<string> RegisterPasswordHasher(UserCredentials credentials)
		{
			var userCheck = await this._authenticationRepository.GetUser(credentials.Email);

			if (userCheck.ToList().Count != 0)
			{
				throw new Exception("User already registered");
				//throw new NullReferenceException("User already registered");
			}

			return this._passwordHasher.Hash(credentials.Password);
		}
	}
}
