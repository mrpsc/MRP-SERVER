using AspNet.Identity.MongoDB;
using Microsoft.AspNet.Identity;
using MongoDB.Driver;
using MRP.Common.DTO;
using MRP.Common.IRepositories;
using MRP.DAL.Models;
using MRP.DAL.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Configuration;
using System.Linq;
using MRP.Common.DTO.Pages;
using MongoDB.Bson;

namespace MRP.DAL.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        MongoClient _client;
        IMongoDatabase _database;
        IMongoCollection<User> _users;
        UserStore<User> _store;
        UserManager<User> _userManager;

        public UsersRepository()
        {
            _client = new MongoClient(ConfigurationManager.ConnectionStrings["Mongo"].ConnectionString);
            _database = _client.GetDatabase(ConfigurationManager.AppSettings.Get("MongoDbName"));
            _users = _database.GetCollection<User>("AspNetUsers");
            _store = new UserStore<User>(_users);
            _userManager = new UserManager<User>(_store);
        }

        public async Task<UsersPage> GetAllUsersAsync(int limit, int skip)
        {
            try
            {
                var query = _users.Find(x => true);
                var totalUsers = await _users.CountAsync(new BsonDocument());
                var users = await query.Skip(skip).Limit(limit).ToListAsync();
                // await Task.WhenAll(totalUsers, users);
                return new UsersPage { Users = users.ConvertToDTOExtension(), Count = Convert.ToInt32(totalUsers) };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<UserDTO> GetUserAsync(string username)
        {
            var collection = await _users.Find(u => u.UserName == username).ToListAsync();
            return collection.ConvertToDTOExtension().ToList()[0];
        }

        public async Task<bool> RemoveUserAsync(string email)
        {
            try
            {
                var user = await _users.FindOneAndDeleteAsync(x => x.Email == email);
                return (user != null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<UserDTO> UpdateUserAsync(UserDTO user)
        {
            try
            {
                var update = new UpdateDefinitionBuilder<User>().Set("Email", user.EmailAddress)
                                                                .Set("Roles", user.Roles)
                                                                .Set("FullName", user.FullName)
                                                                .Set("DateOfBirth", user.DateOfBirth)
                                                                .Set("MadicalInstitution", user.MedicalInstitution.ConvertToModel());
                var userInDb = await _users.FindOneAndUpdateAsync(x => x.Email == user.EmailAddress, update);
                return userInDb.ConvertToDTO();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
