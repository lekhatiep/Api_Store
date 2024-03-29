using Domain.Entities.Identity;
using DoAn3API.Services.Roles;
using Infastructure.Repositories.RoleRepo;
using Infastructure.Repositories.UserRoleRepo;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoAn3API.Dtos.Identity;
using AutoMapper;
using System;
using DoAn3API.DataContext;
using Dapper;

namespace DoAn3API.Services.Roles
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;
        private readonly DapperContext _dapperContext;

        public RoleService(
            IRoleRepository roleRepository,
            IUserRoleRepository userRoleRepository,
            IMapper mapper,
            DapperContext dapperContext
            )
        {
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
            _dapperContext = dapperContext;
        }

        public async Task AssignRoleDefault(string roleName, int userId)
        {
            var role = await _roleRepository.List().Where(x => x.Name.ToLower().Contains(roleName.ToLower().Trim())).FirstOrDefaultAsync();

            if(role == null)
            {
                return;
            }


            var userRole = new UserRole() {
                RoleId = role.Id,
                UserId = userId
            };

            await _userRoleRepository.Insert(userRole);
            await _userRoleRepository.Save();
        }

        public async Task<List<RoleDto>> ListRole()
        {

            var query = "SELECT * FROM Role";
           
            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var listRole = await connection.QueryAsync<RoleDto>(query);

                    if (listRole == null)
                    {
                        return new List<RoleDto>();
                    }

                    return listRole.ToList();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        public async Task AddPermissionToRole(int roleID, List<int> permissionIds)
        {
            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var existingPermissionIds = await connection.QueryAsync<int>(
                           "SELECT ID FROM permission WHERE ID IN @PermissionIds",
                           new { PermissionIds = permissionIds }
                       );

                    var newPermissionIds = permissionIds.Except(existingPermissionIds);

                    var sql = @"INSERT INTO RolePermissions (roleID, permissionID)
                                SELECT @RoleId, ID FROM permission WHERE ID IN @NewPermissionIds";

                     await connection.ExecuteAsync(sql,
                        new { RoleId = roleID, PermissionIds = newPermissionIds }
                    );
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public async Task DeleteRole(int roleID)
        {
            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                   await connection.ExecuteAsync("DELETE FROM role WHERE id = @RoleId", new { RoleId = roleID });
                   await connection.ExecuteAsync("DELETE FROM RolePermissions WHERE id = @RoleId", new { RoleId = roleID });
                }
            }
            catch (Exception)
            {

                throw;
            } 
        }
    }
}
