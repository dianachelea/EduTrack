﻿using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class UserService
	{
		private readonly IPasswordHasher _passwordHasher;
		private readonly IUsersRepository _authenticationRepository;
		private readonly IGenerateToken _generateToken;
		private readonly ITokenRepository _tokenRepository;
		private readonly IUsersRepository _usersRepository;
		private readonly NotificationService _notificationSender;
		private readonly ILinkCreator _linkCreator;
		private readonly ILogger<UserService> _logger;

		public UserService(IPasswordHasher passwordHasher,
			IUsersRepository authenticationRepository,
			IGenerateToken generateToken,
			ITokenRepository tokenRepository,
			IUsersRepository usersRepository,
			NotificationService notificationSender,
			ILinkCreator linkCreator,
			ILogger<UserService> logger)
		{
			_passwordHasher = passwordHasher;
			_authenticationRepository = authenticationRepository;
			_generateToken = generateToken;
			_tokenRepository = tokenRepository;
			_linkCreator = linkCreator;
			_logger = logger;
			_notificationSender = notificationSender;
			_usersRepository = usersRepository;
		}

		public async Task<bool> RegisterUser(UserCredentials credentials)
		{
			var hashedPassword = await RegisterPasswordHasher(credentials);
			var registerResult = await _authenticationRepository.RegisterUser(new UserCredentials
			{
				Username = credentials.Username,
				Password = hashedPassword,
				Email = credentials.Email,
				Role = "user",
				Phone = credentials.Phone,
				FirstName = credentials.FirstName,
				LastName = credentials.LastName
			});

			return registerResult;
		}
		public async Task<bool> RegisterTeacher(UserCredentials credentials)
		{
			var hashedPassword = await RegisterPasswordHasher(credentials);
			var registerResult = await _authenticationRepository.RegisterUser(new UserCredentials
			{
				Username = credentials.Username,
				Password = hashedPassword,
				Email = credentials.Email,
				Role = "teacher",
				Phone = credentials.Phone,
				FirstName = credentials.FirstName,
				LastName = credentials.LastName
			});

			return registerResult;
		}

		//DEV
		public async Task<bool> RegisterAdmin(UserCredentials credentials)
		{
			var hashedPassword = await RegisterPasswordHasher(credentials);
			var registerResult = await _authenticationRepository.RegisterUser(new UserCredentials
			{
				Username = credentials.Username,
				Password = hashedPassword,
				Email = credentials.Email,
				Role = "admin",
				Phone = credentials.Phone,
				FirstName = credentials.FirstName,
				LastName = credentials.LastName
			});

			return registerResult;
		}

		public async Task<bool> ResetPassword(string token, string password)
		{
			var validationToken =  await _tokenRepository.GetToken(token);

			if (validationToken.ToList().Count == 0)
			{
				throw new Exception("Token not found!");
			}

			if (validationToken.FirstOrDefault()?.expirationDate < DateTime.Now)
			{
				throw new Exception("Token expired!");
			}

			var hashedPassword = _passwordHasher.Hash(password);
			var result = await _usersRepository.UpdatePassword(validationToken.FirstOrDefault()?.userEmail, hashedPassword);
			var deleteResult = await _tokenRepository.DeleteToken(token);
			return result;
		}
		public async Task<bool> RecoverPassword(string email)
		{
			var userCheck = await _authenticationRepository.GetUserInfo(email);

			if (userCheck.ToList().Count == 0)
			{
				throw new Exception("User is not registered");
			}

			string token = _generateToken.GenerateToken(32);
			DateTime expiryDate = DateTime.Now.AddMinutes(15);

			var result = await _tokenRepository.AddToken(new ValidationTokenDo
			{
				userEmail = email,
				token = token,
				expirationDate = expiryDate
			});
			string link = _linkCreator.CreateLink("resetpassword?token="+token);
			await _notificationSender.NotifyTeacher(email, link);

			return result;
		}

		public async Task<IEnumerable<UserCredentials>> GetAllStudents()
		{
			var users = await _usersRepository.GetAllStudents();

			return users;
		}

		public async Task<UserInfo> GetUserInfo(string email)
		{
			var user = await _usersRepository.GetUserInfo(email);

			return user.FirstOrDefault();
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
			var userCheck = await _authenticationRepository.GetUserInfo(credentials.Email);

			if (userCheck.ToList().Count != 0)
			{
				throw new Exception("User already registered");
				//throw new NullReferenceException("User already registered");
			}

			return _passwordHasher.Hash(credentials.Password);
		}
	}
}
