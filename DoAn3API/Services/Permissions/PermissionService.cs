using Dapper;
using DoAn3API.DataContext;
using DoAn3API.Dtos.Permission;
using Domain.Entities.Identity;
using Infastructure.Repositories.PermissionRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn3API.Services.Permissions
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly DapperContext _dapperContext;

        public PermissionService(
            IPermissionRepository permissionRepository,
            DapperContext context
            )
        {
            _permissionRepository = permissionRepository;
            _dapperContext = context;
        }
        public  async Task<int> AddNewPermission(string Name)
        {
            try
            {
                var exists = await _permissionRepository.List().Where(x => x.Name.ToLower() == Name.ToLower()).FirstOrDefaultAsync();

                if (exists != null)
                {
                    return 0;
                }

                var permission = new Permission()
                {
                    Name = Name
                };

                await _permissionRepository.Insert(permission);
                await _permissionRepository.Save();

                return 1;
            }
            catch (Exception ex)
            {
                return -1;
                throw;
            }
            
        }

        public async Task<List<PermissionDto>> GetAllPermission()
        {
            var query = "SELECT * FROM Permissions";
            using (var connection = _dapperContext.CreateConnection())
            {
                var permissions = await connection.QueryAsync<PermissionDto>(query);
                return permissions.ToList();
            }
        }

        public async Task<List<string>> GetAllPermissionByRoleId(int id)
        {
            var query = $@"select p.Name from Role r
                        inner join RolePermissions rp on r.Id = rp.RoleId
                        inner join Permissions p on p.Id = rp.PermissionId
                        where r.Id = {id}";

            using (var connection = _dapperContext.CreateConnection())
            {
                var permissions = await connection.QueryAsync<string>(query);
                return permissions.ToList();
            }
        }

        public async Task<List<string>> GetAllPermissionByRoleName(string roleName)
        {
            var query = $@"select p.Name from Role r
                        inner join RolePermissions rp on r.Id = rp.RoleId
                        inner join Permissions p on p.Id = rp.PermissionId
                        where r.Name = {roleName}";

            using (var connection = _dapperContext.CreateConnection())
            {
                var permissions = await connection.QueryAsync<string>(query);
                return permissions.ToList();
            }
        }
    }
}
