using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;
using Domain.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Diagnostics;

namespace Application.Handlers
{
    public class DuplicateFileErrorHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is DuplicateFileException duplicateFileException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status409Conflict; // Conflict
                await httpContext.Response.WriteAsync(duplicateFileException.Message, cancellationToken);
                return true;
            }

            return false;
        }
    }

    public class FileTooLargeErrorHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is FileTooLargeException fileTooLargeException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status413PayloadTooLarge; // Payload Too Large
                await httpContext.Response.WriteAsync(fileTooLargeException.Message, cancellationToken);
                return true;
            }

            return false;
        }
    }

    public class FileTypeNotSupportedErrorHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is FileTypeNotSupportedException fileTypeNotSupportedException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType; // Unsupported Media Type
                await httpContext.Response.WriteAsync(fileTypeNotSupportedException.Message, cancellationToken);
                return true;
            }

            return false;
        }
    }

    public class FileNotAvailableErrorHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is FileNotAvailableException fileNotAvailableException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound; // Not Found
                await httpContext.Response.WriteAsync(fileNotAvailableException.Message, cancellationToken);
                return true;
            }

            return false;
        }
    }
}