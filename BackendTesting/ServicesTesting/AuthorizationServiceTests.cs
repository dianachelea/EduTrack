using Application.Interfaces;
using Application.Services;
using Domain;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class AuthorizationServiceTests
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAuthenticationRepository _authenticationRepository;
    private readonly IIdentityHandler _identityHandler;
    private readonly AuthorizationService _authorizationService;

    public AuthorizationServiceTests()
    {
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _authenticationRepository = Substitute.For<IAuthenticationRepository>();
        _identityHandler = Substitute.For<IIdentityHandler>();
        _authorizationService = new AuthorizationService(_passwordHasher, _authenticationRepository, _identityHandler);
    }

    [Fact]
    public async Task LoginUser_ShouldReturnUser_WhenCredentialsAreValid()
    {
        // Arrange
        var credentials = new UserCredentials { Email = "test@example.com", Password = "password" };
        var userCredentials = new List<UserCredentials>
        {
            new UserCredentials { Username = "testuser", Password = "hashedPassword", Email = "test@example.com", Role = "user" }
        };
        var expectedUser = new User { Username = "testuser", Email = "test@example.com", Role = "user" };

        _authenticationRepository.GetUser(credentials.Email).Returns(userCredentials);
        _passwordHasher.Verify(userCredentials[0].Password, credentials.Password).Returns(true);
        _identityHandler.GenerateToken(Arg.Any<User>()).Returns("jwtToken");

        // Act
        var result = await _authorizationService.LoginUser(credentials);

        // Assert
        result.Should().BeEquivalentTo(expectedUser, options => options.Excluding(u => u.JwtToken));
        result.JwtToken.Should().Be("jwtToken");
    }

    [Fact]
    public async Task LoginUser_ShouldThrowException_WhenCredentialsAreInvalid()
    {
        // Arrange
        var credentials = new UserCredentials { Email = "test@example.com", Password = "password" };
        var userCredentials = new List<UserCredentials>
        {
            new UserCredentials { Username = "testuser", Password = "hashedPassword", Email = "test@example.com", Role = "user" }
        };

        _authenticationRepository.GetUser(credentials.Email).Returns(userCredentials);
        _passwordHasher.Verify(userCredentials[0].Password, credentials.Password).Returns(false);

        // Act
        Func<Task> act = async () => await _authorizationService.LoginUser(credentials);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Username or password are incorrect");
    }

    [Fact]
    public async Task GiveUserAdminRights_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange
        var email = "test@example.com";
        var userCredentials = new List<UserCredentials>
        {
            new UserCredentials { Email = email }
        };

        _authenticationRepository.GetUser(email).Returns(userCredentials);
        _authenticationRepository.GiveUserAdminRights(email).Returns(true);

        // Act
        var result = await _authorizationService.GiveUserAdminRights(email);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task GiveUserAdminRights_ShouldThrowException_WhenUserDoesNotExist()
    {
        // Arrange
        var email = "test@example.com";

        _authenticationRepository.GetUser(email).Returns(new List<UserCredentials>());

        // Act
        Func<Task> act = async () => await _authorizationService.GiveUserAdminRights(email);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("User is not registered");
    }
}
