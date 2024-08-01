using Application.Interfaces;
using Application.Services;
using Domain;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class UserServiceTests
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUsersRepository _authenticationRepository;
    private readonly IGenerateToken _generateToken;
    private readonly ITokenRepository _tokenRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly ISendNotification _sendNotification;
    private readonly NotificationService _notificationSender;
    private readonly ILinkCreator _linkCreator;
    private readonly ILogger<UserService> _logger;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _authenticationRepository = Substitute.For<IUsersRepository>();
        _generateToken = Substitute.For<IGenerateToken>();
        _tokenRepository = Substitute.For<ITokenRepository>();
        _usersRepository = Substitute.For<IUsersRepository>();
        _sendNotification = Substitute.For<ISendNotification>();
        _notificationSender = new NotificationService(_sendNotification);
        _linkCreator = Substitute.For<ILinkCreator>();
        _logger = Substitute.For<ILogger<UserService>>();
        _userService = new UserService(
            _passwordHasher,
            _authenticationRepository,
            _generateToken,
            _tokenRepository,
            _usersRepository,
            _notificationSender,
            _linkCreator,
            _logger
        );
    }

    [Fact]
    public async Task RegisterUser_ShouldReturnTrue_WhenRegistrationIsSuccessful()
    {
        var credentials = new UserCredentials
        {
            Username = "testuser",
            Password = "password",
            Email = "testuser@example.com",
            Phone = "123456789",
            FirstName = "Test",
            LastName = "User"
        };

        _passwordHasher.Hash(credentials.Password).Returns("hashedpassword");
        _authenticationRepository.RegisterUser(Arg.Any<UserCredentials>()).Returns(true);

        var result = await _userService.RegisterUser(credentials);

        result.Should().BeTrue();
        await _authenticationRepository.Received(1).RegisterUser(Arg.Is<UserCredentials>(x => x.Password == "hashedpassword"));
    }

    [Fact]
    public async Task RegisterUser_ShouldThrowException_WhenUserIsAlreadyRegistered()
    {
        var credentials = new UserCredentials
        {
            Username = "testuser",
            Password = "password",
            Email = "testuser@example.com",
            Phone = "123456789",
            FirstName = "Test",
            LastName = "User"
        };

        _authenticationRepository.GetUserInfo(credentials.Email).Returns(new List<UserCredentials> { credentials });

        Func<Task> act = async () => await _userService.RegisterUser(credentials);

        await act.Should().ThrowAsync<Exception>().WithMessage("User already registered");
    }

    //[Fact]
    //public async Task ResetPassword_ShouldReturnTrue_WhenTokenIsValidAndPasswordIsUpdated()
    //{
    //    var token = "validtoken";
    //    var password = "newpassword";
    //    var validationToken = new List<ValidationTokenDo> { new ValidationTokenDo { userEmail = "user@example.com", expirationDate = DateTime.Now.AddMinutes(10) } };

    //    _tokenRepository.GetToken(token).Returns(validationToken);
    //    _passwordHasher.Hash(password).Returns("hashedpassword");
    //    _usersRepository.UpdatePassword(validationToken[0].userEmail, "hashedpassword").Returns(true);
    //    _tokenRepository.DeleteToken(token).Returns(true);

    //    var result = await _userService.ResetPassword(token, password);

    //    result.Should().BeTrue();
    //}

    [Fact]
    public async Task ResetPassword_ShouldThrowException_WhenTokenIsExpired()
    {
        var token = "expiredtoken";
        var password = "newpassword";
        var validationToken = new List<ValidationTokenDo> { new ValidationTokenDo { userEmail = "user@example.com", expirationDate = DateTime.Now.AddMinutes(-10) } };

        _tokenRepository.GetToken(token).Returns(validationToken);

        Func<Task> act = async () => await _userService.ResetPassword(token, password);

        await act.Should().ThrowAsync<Exception>().WithMessage("Token expired!");
    }

    [Fact]
    public async Task RecoverPassword_ShouldReturnTrue_WhenUserIsRegistered()
    {
        var email = "user@example.com";
        var token = "generatedtoken";
        var link = "resetpasswordlink";
        var notificationMessage = "EduTrack - Empower your learning, Achieve your goals!\n\n" + link + "\n\n\tThank you, \n\tEduTrack Team!";

        _authenticationRepository.GetUserInfo(email).Returns(new List<UserCredentials> { new UserCredentials { Email = email } });
        _generateToken.GenerateToken(32).Returns(token);
        _linkCreator.CreateLink(Arg.Any<string>()).Returns(link);
        _tokenRepository.AddToken(Arg.Any<ValidationTokenDo>()).Returns(true);
        _sendNotification.SendNotificationTo(email, notificationMessage).Returns(Task.FromResult(true));

        var result = await _userService.RecoverPassword(email);

        result.Should().BeTrue();
        await _tokenRepository.Received(1).AddToken(Arg.Any<ValidationTokenDo>());
        await _sendNotification.Received(1).SendNotificationTo(email, notificationMessage);
    }

    [Fact]
    public async Task RecoverPassword_ShouldThrowException_WhenUserIsNotRegistered()
    {
        var email = "unregistered@example.com";

        _authenticationRepository.GetUserInfo(email).Returns(new List<UserCredentials>());

        Func<Task> act = async () => await _userService.RecoverPassword(email);

        await act.Should().ThrowAsync<Exception>().WithMessage("User is not registered");
    }

    //[Fact]
    //public async Task GetAllStudents_ShouldReturnAllStudents()
    //{
    //    var students = new List<UserCredentials>
    //    {
    //        new UserCredentials { Username = "student1", Email = "student1@example.com" },
    //        new UserCredentials { Username = "student2", Email = "student2@example.com" }
    //    };

    //    _usersRepository.GetAllStudents().Returns(students);

    //    var result = await _userService.GetAllStudents();

    //    result.Should().BeEquivalentTo(students);
    //}
}
