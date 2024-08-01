using Application.Interfaces;
using Application.Services;
using FluentAssertions;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

public class NotificationServiceTests
{
    private readonly ISendNotification _notificationSender;
    private readonly NotificationService _notificationService;

    public NotificationServiceTests()
    {
        _notificationSender = Substitute.For<ISendNotification>();
        _notificationService = new NotificationService(_notificationSender);
    }

    [Fact]
    public async Task NotifyTeacher_ShouldReturnTrue_WhenNotificationIsSentSuccessfully()
    {
        // Arrange
        var email = "teacher@example.com";
        var notificationMessage = "You have a new lesson to review.";
        _notificationSender.SendNotificationTo(email, Arg.Any<string>()).Returns(true);

        // Act
        var result = await _notificationService.NotifyTeacher(email, notificationMessage);

        // Assert
        result.Should().BeTrue();
        await _notificationSender.Received(1).SendNotificationTo(email, Arg.Is<string>(body => body.Contains(notificationMessage)));
    }

    [Fact]
    public async Task NotifyTeacher_ShouldReturnFalse_WhenNotificationFailsToSend()
    {
        // Arrange
        var email = "teacher@example.com";
        var notificationMessage = "You have a new lesson to review.";
        _notificationSender.SendNotificationTo(email, Arg.Any<string>()).Returns(false);

        // Act
        var result = await _notificationService.NotifyTeacher(email, notificationMessage);

        // Assert
        result.Should().BeFalse();
    }
}
