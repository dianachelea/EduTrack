using Application.Interfaces;
using Application.Services;
using Domain;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class FeedbackServiceTests
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly FeedbackService _feedbackService;

    public FeedbackServiceTests()
    {
        _feedbackRepository = Substitute.For<IFeedbackRepository>();
        _feedbackService = new FeedbackService(_feedbackRepository);
    }

    [Fact]
    public async Task AddFeedback_ShouldReturnTrue_WhenFeedbackIsValid()
    {
        // Arrange
        var feedback = new FeedbackDO
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Title = "Great Course",
            Content = "I really enjoyed the course.",
            Stars = 5,
            IsAnonymus = false,
            Category = FeedbackCategory.ContentQuality
        };
        _feedbackRepository.AddFeedback(Arg.Any<FeedbackDO>()).Returns(true);

        // Act
        var result = await _feedbackService.AddFeedback(feedback);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task AddFeedback_ShouldThrowException_WhenEmailIsInvalid()
    {
        // Arrange
        var feedback = new FeedbackDO
        {
            Name = "John Doe",
            Email = "invalid-email",
            Title = "Great Course",
            Content = "I really enjoyed the course.",
            Stars = 5,
            IsAnonymus = false,
            Category = FeedbackCategory.ContentQuality
        };

        // Act
        Func<Task> act = async () => await _feedbackService.AddFeedback(feedback);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Email invalid!");
    }

    [Fact]
    public async Task GetFeedback_ShouldReturnAllFeedbacks_WhenNoFiltersAreApplied()
    {
        // Arrange
        var feedbacks = new List<FeedbackDO>
        {
            new FeedbackDO { Name = "John Doe", Email = "john.doe@example.com", Title = "Great Course", Content = "I really enjoyed the course.", Stars = 5, IsAnonymus = false, Category = FeedbackCategory.ContentQuality }
        };
        _feedbackRepository.GetFeedback().Returns(feedbacks);

        // Act
        var result = await _feedbackService.GetFeedback(new FeedbackFilters());

        // Assert
        result.Should().BeEquivalentTo(feedbacks);
    }

    [Fact]
    public async Task GetFeedback_ShouldThrowException_WhenEmailFilterIsInvalid()
    {
        // Arrange
        var filters = new FeedbackFilters
        {
            ByEmail = new List<string> { "invalid-email" }
        };

        // Act
        Func<Task> act = async () => await _feedbackService.GetFeedback(filters);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Email invalid!");
    }

    [Fact]
    public async Task GetFeedback_ShouldReturnFilteredFeedbacks_WhenFiltersAreApplied()
    {
        // Arrange
        var filters = new FeedbackFilters
        {
            ByEmail = new List<string> { "john.doe@example.com" }
        };
        var feedbacks = new List<FeedbackDO>
        {
            new FeedbackDO { Name = "John Doe", Email = "john.doe@example.com", Title = "Great Course", Content = "I really enjoyed the course.", Stars = 5, IsAnonymus = false, Category = FeedbackCategory.ContentQuality }
        };
        _feedbackRepository.GetFeedback(filters).Returns(feedbacks);

        // Act
        var result = await _feedbackService.GetFeedback(filters);

        // Assert
        result.Should().BeEquivalentTo(feedbacks);
    }
}
