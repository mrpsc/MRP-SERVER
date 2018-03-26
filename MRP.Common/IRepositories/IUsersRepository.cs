using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRP.Common.DTO;
using MRP.Common.DTO.Pages;

namespace MRP.Common.IRepositories
{
    public interface IUsersRepository
    {
        Task<UsersPage> GetAllUsersAsync(int limit, int skip);
        Task<UserDTO> GetUserAsync(string username);
        Task<UserDTO> UpdateUserAsync(UserDTO user);
        Task<bool> RemoveUserAsync(string email);
    }
}
