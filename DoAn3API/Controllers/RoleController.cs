using DoAn3API.Services.Roles;
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
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(
            IRoleService roleService    
        )
        {
            _roleService = roleService;
        }
        // GET: api/<RoleController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("GetListRole")]
        public async Task<ActionResult> GetListRole()
        {
            try
            {
                var listRole = await _roleService.ListRole();
                return Ok(listRole);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }

        // GET api/<RoleController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RoleController>
        [HttpPost("AddPermissionToRole")]
        public async Task<ActionResult> AddPermissionToRole(int roleID, [FromBody] List<int> permissionIDs)
        {
            try
            {
                await _roleService.AddPermissionToRole(roleID, permissionIDs);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }

        // PUT api/<RoleController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RoleController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int roleID)
        {
            try
            {
                await _roleService.DeleteRole(roleID);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }
    }
}
