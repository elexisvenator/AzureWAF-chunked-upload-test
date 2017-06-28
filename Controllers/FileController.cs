namespace ChunkedUploadTest.Controllers
{
    using System;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    [Produces("application/json")]
    [Route("api/File/Upload")]
    public class FileController : Controller
    {
        private readonly IMemoryCache cache;

        public FileController(IMemoryCache cache)
        {
            this.cache = cache;
        }

        [HttpGet]
        public IActionResult Get(Guid resumableIdentifier, int resumableChunkNumber)
        {
            return this.cache.TryGetValue($"{resumableIdentifier}-{resumableChunkNumber}", out var _)
                       ? this.StatusCode(200)
                       : this.StatusCode(204);
        }

        [HttpPost]
        public IActionResult Post(Guid resumableIdentifier, int resumableChunkNumber, IFormFile chunk)
        {
            this.cache.Set($"{resumableIdentifier}-{resumableChunkNumber}", true);
            return this.StatusCode(200);
        }
    }
}