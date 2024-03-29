using Dapper;
using DoAn3API.DataContext;
using DoAn3API.Dtos.Permission;
using Domain.Entities.Identity;
using Infastructure.Repositories.PermissionRepo;
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
            throw new NotImplementedException();
        }
    }
}
