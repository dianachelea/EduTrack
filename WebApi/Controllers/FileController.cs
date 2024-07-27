using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Application.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FileController : ControllerBase
    {
        private readonly FileService _fileService;

        public FileController(FileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetFile(string fileName)
        {
            var result = await this._fileService.GetFile(fileName);

            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> SaveFile(IFormFile file)
        {
            var result = await this._fileService.SaveFile(file);

            return Ok(result);
        }
    }
}