using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Xunit;
using Application.Handlers;
using Domain.Exceptions;

public class ErrorHandlerTests
{
    //[Fact]
    //public async Task DuplicateFileErrorHandler_ShouldHandleDuplicateFileException()
    //{
    //    // Arrange
    //    var httpContext = Substitute.For<HttpContext>();
    //    var response = Substitute.For<HttpResponse>();
    //    httpContext.Response.Returns(response);
    //    var handler = new DuplicateFileErrorHandler();
    //    var exception = new DuplicateFileException("Duplicate file error");

    //    // Act
    //    var result = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

    //    // Assert
    //    Assert.True(result);
    //    httpContext.Response.Received().StatusCode = StatusCodes.Status409Conflict;
    //    await httpContext.Response.Received().WriteAsync("Duplicate file error", CancellationToken.None);
    //}

    //[Fact]
    //public async Task FileTooLargeErrorHandler_ShouldHandleFileTooLargeException()
    //{
    //    // Arrange
    //    var httpContext = Substitute.For<HttpContext>();
    //    var response = Substitute.For<HttpResponse>();
    //    httpContext.Response.Returns(response);
    //    var handler = new FileTooLargeErrorHandler();
    //    var exception = new FileTooLargeException("File too large error");

    //    // Act
    //    var result = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

    //    // Assert
    //    Assert.True(result);
    //    httpContext.Response.Received().StatusCode = StatusCodes.Status413PayloadTooLarge;
    //    await httpContext.Response.Received().WriteAsync("File too large error", CancellationToken.None);
    //}

    ////[Fact]
    ////public async Task FileTypeNotSupportedErrorHandler_ShouldHandleFileTypeNotSupportedException()
    ////{
    ////    // Arrange
    ////    var httpContext = Substitute.For<HttpContext>();
    ////    var response = Substitute.For<HttpResponse>();
    ////    httpContext.Response.Returns(response);
    ////    var handler = new FileTypeNotSupportedErrorHandler();
    ////    var exception = new FileTypeNotSupportedException("File type not supported error");

    ////    // Act
    ////    var result = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

    ////    // Assert
    ////    Assert.True(result);
    ////    httpContext.Response.Received().StatusCode = StatusCodes.Status415UnsupportedMediaType;
    ////    await httpContext.Response.Received().WriteAsync("File type not supported error", CancellationToken.None);
    ////}

    //[Fact]
    //public async Task FileNotAvailableErrorHandler_ShouldHandleFileNotAvailableException()
    //{
    //    // Arrange
    //    var httpContext = Substitute.For<HttpContext>();
    //    var response = Substitute.For<HttpResponse>();
    //    httpContext.Response.Returns(response);
    //    var handler = new FileNotAvailableErrorHandler();
    //    var exception = new FileNotAvailableException("File not available error");

    //    // Act
    //    var result = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

    //    // Assert
    //    Assert.True(result);
    //    httpContext.Response.Received().StatusCode = StatusCodes.Status404NotFound;
    //    await httpContext.Response.Received().WriteAsync("File not available error", CancellationToken.None);
    //}

    [Fact]
    public async Task DuplicateFileErrorHandler_ShouldReturnFalse_ForNonDuplicateFileException()
    {
        // Arrange
        var httpContext = Substitute.For<HttpContext>();
        var handler = new DuplicateFileErrorHandler();
        var exception = new System.Exception("General error");

        // Act
        var result = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task FileTooLargeErrorHandler_ShouldReturnFalse_ForNonFileTooLargeException()
    {
        // Arrange
        var httpContext = Substitute.For<HttpContext>();
        var handler = new FileTooLargeErrorHandler();
        var exception = new System.Exception("General error");

        // Act
        var result = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task FileTypeNotSupportedErrorHandler_ShouldReturnFalse_ForNonFileTypeNotSupportedException()
    {
        // Arrange
        var httpContext = Substitute.For<HttpContext>();
        var handler = new FileTypeNotSupportedErrorHandler();
        var exception = new System.Exception("General error");

        // Act
        var result = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task FileNotAvailableErrorHandler_ShouldReturnFalse_ForNonFileNotAvailableException()
    {
        // Arrange
        var httpContext = Substitute.For<HttpContext>();
        var handler = new FileNotAvailableErrorHandler();
        var exception = new System.Exception("General error");

        // Act
        var result = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        // Assert
        Assert.False(result);
    }
}
