namespace ChunkedUploadTest.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        public IActionResult Error()
        {
            return this.View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Index(IFormFile uploadedFile)
        {
            return this.RedirectToAction("Index");
        }
    }
}