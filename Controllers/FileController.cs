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

        /// <summary>
        /// Check to see if a chunk has already been uploaded.
        /// </summary>
        /// <param name="resumableIdentifier">
        /// The unique identifier for this file.</param>
        /// <param name="resumableChunkNumber">
        /// The index of the current chunk.</param>
        /// <returns>
        /// <code>200 OK</code> if the specified chunk is already uploaded, otherwise <code>204 No content</code>.
        /// </returns>
        [HttpGet]
        public IActionResult Get(Guid resumableIdentifier, int resumableChunkNumber)
        {
            // Check the memory cache to see if a reference for this chunk already exists.
            return this.cache.TryGetValue($"{resumableIdentifier}-{resumableChunkNumber}", out var _)
                       ? this.StatusCode(200)
                       : this.StatusCode(204);
        }

        /// <summary>
        /// Receive the uploaded chunk file and store it.
        /// If all chunks are received then assemble the file.
        /// </summary>
        /// <param name="resumableIdentifier">
        /// The unique identifier for this file.
        /// </param>
        /// <param name="resumableChunkNumber">
        /// The index of the current chunk.
        /// </param>
        /// <param name="chunk">
        /// The file chunk.
        /// </param>
        /// <returns>
        /// <code>200 OK </code> to indicate the the chunk was successfully uploaded.
        /// </returns>
        [HttpPost]
        public IActionResult Post(Guid resumableIdentifier, int resumableChunkNumber, IFormFile chunk)
        {
            // As we are not actually storing the file in this example, instead just remember that we have received this hunk by storing it's id in a memory cache.
            this.cache.Set($"{resumableIdentifier}-{resumableChunkNumber}", true);
            return this.StatusCode(200);
        }
    }
}