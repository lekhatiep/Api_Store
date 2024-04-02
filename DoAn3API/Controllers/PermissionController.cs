using DoAn3API.Authorize.CustomAuthorize;
using DoAn3API.Constants;
using DoAn3API.Services.Permissions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DoAn3API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(
            IPermissionService permissionService
            )
        {
            _permissionService = permissionService;
        }
        // GET: api/<PermissionController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [CustomAuthorize(NamePermissions.Permission.View)]
        [HttpGet("GetListPermission")]
        public async Task<ActionResult> GetListPermission()
        {
            try
            {
                var rs = await _permissionService.GetAllPermission();
                return Ok(rs);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [CustomAuthorize(NamePermissions.Permission.View)]
        [HttpGet("GetPermissionByRoleID")]
        public async Task<ActionResult> GetPermissionByRoleID(int id)
        {
            try
            {
                var rs = await _permissionService.GetAllPermissionByRoleId(id);
                return Ok(rs);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [CustomAuthorize(NamePermissions.Permission.View)]
        [HttpGet("GetPermissionByRoleName")]
        public async Task<ActionResult> GetPermissionByRoleName(string name)
        {
            try
            {
                var rs = await _permissionService.GetAllPermissionByRoleName(name);
                return Ok(rs);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        // GET api/<PermissionController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PermissionController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [CustomAuthorize(NamePermissions.Permission.Create)]
        [HttpPost("AddPermission")]
        public async Task<ActionResult> AddPermission([FromBody] string Name)
        {
            try
            {
                var rs = await _permissionService.AddNewPermission(Name);
                return Ok(rs);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpGet("SyncNamePermission")]
        [CustomAuthorize(NamePermissions.Permission.Create)]
        public async Task<ActionResult> SyncNamePermission()
        {
            try
            {
                await _permissionService.SyncNamePermission();
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        // PUT api/<PermissionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PermissionController>/5
        [CustomAuthorize(NamePermissions.Permission.Delete)]
        [HttpDelete("{id}")]
        public async Task<ActionResult>  Delete(int id)
        {
            try
            {
                await _permissionService.DeletePermission(id);
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
    }
}
