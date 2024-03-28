using DoAn3API.Dto.Firebase;
using DoAn3API.Services.Firebase;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DoAn3API.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFirebaseService firebaseService;

        public HomeController(IFirebaseService firebaseService)
        {
            this.firebaseService = firebaseService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("upload")]
        public async Task<ActionResult> TestUploadFirebase([FromForm] FileUploadDto uploadDto)
        {
            try
            {
                //string json = System.Text.Json.JsonSerializer.Serialize(uploadDto);
                var ps = await firebaseService.UploadFileAsync(uploadDto);
                return Ok(ps);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
    }
}
