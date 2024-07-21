using System;

namespace Domain.Exceptions
{
    public class DuplicateFileException : Exception
    {
        public DuplicateFileException(string fileName)
            : base($"A file with the name '{fileName}' already exists.")
        {
        }
    }

    public class FileTooLargeException : Exception
    {
        public FileTooLargeException(long maxSize)
            : base($"The file exceeds the maximum allowed size of {maxSize} bytes.")
        {
        }
    }

    public class FileTypeNotSupportedException : Exception
    {
        public FileTypeNotSupportedException(string fileType)
            : base($"The file type '{fileType}' is not supported.")
        {
        }
    }

    public class FileNotAvailableException : Exception
    {
        public FileNotAvailableException(string fileName)
            : base($"The file '{fileName}' is not available.")
        {
        }
    }
}