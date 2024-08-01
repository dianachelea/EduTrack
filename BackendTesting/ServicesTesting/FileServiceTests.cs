using Application.Interfaces;
using Application.Services;
using Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class FileServiceTests
{
    private readonly IFileRepository _fileRepository;
    private readonly FileService _fileService;

    public FileServiceTests()
    {
        _fileRepository = Substitute.For<IFileRepository>();
        _fileService = new FileService(_fileRepository);
    }

    [Fact]
    public async Task GetFile_ShouldReturnFileContentResult_WhenFileExists()
    {
        // Arrange
        var fileName = "test.txt";
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
        var fileDetails = new FileDetails { FileName = fileName, Path = filePath };
        _fileRepository.GetFile(fileName).Returns(new List<FileDetails> { fileDetails });

        await File.WriteAllTextAsync(filePath, "Test content");

        // Act
        var result = await _fileService.GetFile(fileName);

        // Assert
        result.Should().BeOfType<FileContentResult>();
        result.FileDownloadName.Should().Be(fileName);

        // Clean up
        File.Delete(filePath);
    }

    [Fact]
    public async Task GetFile_ShouldThrowException_WhenFileDoesNotExist()
    {
        // Arrange
        var fileName = "nonexistent.txt";
        _fileRepository.GetFile(fileName).Returns(Enumerable.Empty<FileDetails>());

        // Act
        Func<Task> act = async () => await _fileService.GetFile(fileName);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("File does not exist");
    }

    //[Fact]
    //public async Task SaveFile_ShouldReturnTrue_WhenFileIsSavedSuccessfully()
    //{
    //    // Arrange
    //    var fileName = "test.txt";
    //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Domain", "files", fileName);
    //    var fileMock = Substitute.For<IFormFile>();
    //    fileMock.FileName.Returns(fileName);
    //    fileMock.Length.Returns(1024);

    //    _fileRepository.GetFile(fileName).Returns(Enumerable.Empty<FileDetails>());
    //    _fileRepository.SaveFile(fileName, filePath).Returns(true);

    //    // Act
    //    var result = await _fileService.SaveFile(fileMock);

    //    // Assert
    //    result.Should().BeTrue();
    //}

    [Fact]
    public async Task SaveFile_ShouldThrowException_WhenFileExtensionIsInvalid()
    {
        // Arrange
        var fileName = "test.invalid";
        var fileMock = Substitute.For<IFormFile>();
        fileMock.FileName.Returns(fileName);

        // Act
        Func<Task> act = async () => await _fileService.SaveFile(fileMock);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Invalid file extension");
    }

    //[Fact]
    //public async Task SaveFile_ShouldThrowException_WhenFileSizeIsTooBig()
    //{
    //    // Arrange
    //    var fileName = "test.txt";
    //    var fileMock = Substitute.For<IFormFile>();
    //    fileMock.FileName.Returns(fileName);
    //    fileMock.Length.Returns(26 * 1024 * 1024); // 26 MB

    //    // Act
    //    Func<Task> act = async () => await _fileService.SaveFile(fileMock);

    //    // Assert
    //    await act.Should().ThrowAsync<Exception>().WithMessage("File size is too big");
    //}

    //[Fact]
    //public async Task SaveFile_ShouldThrowException_WhenFileAlreadyExists()
    //{
    //    // Arrange
    //    var fileName = "test.txt";
    //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Domain", "files", fileName);
    //    var fileMock = Substitute.For<IFormFile>();
    //    fileMock.FileName.Returns(fileName);
    //    fileMock.Length.Returns(1024);

    //    var fileDetails = new FileDetails { FileName = fileName, Path = filePath };
    //    _fileRepository.GetFile(fileName).Returns(new List<FileDetails> { fileDetails });

    //    // Act
    //    Func<Task> act = async () => await _fileService.SaveFile(fileMock);

    //    // Assert
    //    await act.Should().ThrowAsync<Exception>().WithMessage("File already exists");
    //}
}
