﻿using Application.Interfaces;
using Domain;
using System.Security.Claims;

namespace Application.Services
{
    public class AuthorizationService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IIdentityHandler _identityHandler;

        public AuthorizationService(IPasswordHasher passwordHasher,
            IAuthenticationRepository authenticationRepository,
            IIdentityHandler identityHandler)
        {
            _passwordHasher = passwordHasher;
            _identityHandler = identityHandler;
            _authenticationRepository = authenticationRepository;
        }

        public async Task<User> LoginUser(UserCredentials credentials)
        {
            var userHashed = await this._authenticationRepository.GetUser(credentials.Email);

            if (!_passwordHasher.Verify(userHashed.FirstOrDefault().Password, credentials.Password))
            {
                throw new Exception("Username or password are incorrect");
            }

            var result = new User
            {
                Username = userHashed.FirstOrDefault().Username,
                Email = userHashed.FirstOrDefault().Email,
                Role = userHashed.FirstOrDefault().Role,
            };

            var jwtToken = this._identityHandler.GenerateToken(result);
            result.JwtToken = jwtToken;

            return result;
        }

        public async Task<bool> GiveUserAdminRights(string email)
        {
            var userCheck = await this._authenticationRepository.GetUser(email);

            if (userCheck.ToList().Count == 0)
            {
                throw new Exception("User is not registered");
            }

            var result = await this._authenticationRepository.GiveUserAdminRights(email);

            return result;
        }
    }
}
