using DoAn3API.Dto.Firebase;
using DoAn3API.Dtos.Permission;
using DoAn3API.Services.Firebase;
using DoAn3API.Services.Permissions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DoAn3API.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFirebaseService firebaseService;
        private readonly IPermissionService _permissionService;

        public HomeController(
            IFirebaseService firebaseService,
            IPermissionService permissionService
            )
        {
            this.firebaseService = firebaseService;
            _permissionService = permissionService;
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
