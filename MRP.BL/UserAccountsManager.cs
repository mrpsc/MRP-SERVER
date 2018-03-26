﻿using Microsoft.AspNet.Identity;
using MRP.Common.DTO;
using MRP.Common.DTO.Pages;
using MRP.Common.IRepositories;
using MRP.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MRP.BL
{
    public class UserAccountsManager
    {
        IAuthRpository authRep;
        IUsersRepository userRep;
        public UserAccountsManager(IAuthRpository ar,IUsersRepository ur)
        {
            authRep = ar;
            userRep = ur;
        }
        public UserAccountsManager()
        {
            authRep = new AuthRepository();
            userRep = new UsersRepository();
        }
        
        public Task<UserDTO> Login(string username, string password)
        {
            return authRep.Login(username,password);
        }

        public Task<IdentityResult> CreateAsync(RegistrationInfo regInfo)
        {
            return authRep.Register(regInfo);
        }

        public async Task<UsersPage> GetAllUsersAsync(int limit, int skip)
        {
            return await userRep.GetAllUsersAsync(limit, skip);
        }

        public Task<UserDTO> GetUserAsync(string username)
        {
            return userRep.GetUserAsync(username);
        }

        public Task<bool> RecoverPasswordAsync(RecoveryInfo recInfo)
        {
            return authRep.RecoverPasswordAsync(recInfo);
        }

        public async Task<UserDTO> UpdateUserAsync(UserDTO user)
        {
            return await userRep.UpdateUserAsync(user);
        }

        public async Task<bool> RemoveUserAsync(string email)
        {
            return await userRep.RemoveUserAsync(email);
        }
    }
}
