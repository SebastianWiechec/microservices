using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UserApi.Data;
using UserApi.IServices;
using UserApi.Models;

namespace UserApi.Sevices
{
    public class UserService : IUserService
    {
        ApplicationDbContext dbContext;
        private UserManager<IdentityUser> userManager { get; }

        public UserService(ApplicationDbContext _db, UserManager<IdentityUser> _userManager)
        {
            dbContext = _db;
            userManager = _userManager;
        }
        
        /// <summary>
        /// Metoda zwracająca wszystkich użytkowników
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<IdentityUser>> GetUsers()
        {
            return await dbContext.Users.ToListAsync();
        }

        /// <summary>
        /// metoda dodająca użytkownika
        /// </summary>
        /// <param name="user">Obietk użyt</param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<string> AddUser(IdentityUser user, string password)
        {
            return await Register(user, password);
        }

        /// <summary>
        /// medota edytowania użytkownika
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ExpandoObject> UpdateUser(ExpandoObject user)
        {
            string id = user.Where(u => u.Key == "idUser").FirstOrDefault().Value.ToString();
            var newUser = userManager.FindByIdAsync(id).Result;

            newUser.UserName = user.Where(u => u.Key == "UserName").FirstOrDefault().Value.ToString();
            newUser.Email = user.Where(u => u.Key == "Email").FirstOrDefault().Value.ToString();
            newUser.PhoneNumber = user.Where(u => u.Key == "PhoneNumber").FirstOrDefault().Value.ToString();

            var userAddress = new Address();
            userAddress.AspNetUsersID = id;
            userAddress.FirstName = user.Where(u => u.Key == "FirstName").FirstOrDefault().Value.ToString();
            userAddress.LastName = user.Where(u => u.Key == "LastName").FirstOrDefault().Value.ToString();
            userAddress.City = user.Where(u => u.Key == "City").FirstOrDefault().Value.ToString();
            userAddress.Country = user.Where(u => u.Key == "Country").FirstOrDefault().Value.ToString();
            userAddress.PostCode = user.Where(u => u.Key == "PostCode").FirstOrDefault().Value.ToString();

            //dbContext.Adresses.Update(userAddress);
            try
            {
                dbContext.Entry(userAddress).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();

                await userManager.UpdateAsync(newUser);
            }
            catch (Exception ex)
            {
                var userR = (IDictionary<string, object>)user;
                userR.Add("Ex", ex);
                return (ExpandoObject)userR;
            }

            return user;
        }

        /// <summary>
        /// metoda usuwająca użytkownika
        /// </summary>
        /// <param name="id"></param>
        /// <returns>W</returns>
        public async Task<string> DeleteUser(string id)
        {
            var user = dbContext.Users.FirstOrDefault(x => x.Id == id);
            dbContext.Entry(user).State = EntityState.Deleted;
            dbContext.SaveChanges();
            return await Task.FromResult("");
        }

        /// <summary>
        /// metoda zwracająca użytkownika według {id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IDictionary<string, object>> GetUserById(string id)
        {
            dynamic user = new ExpandoObject();
            var userDictionary = (IDictionary<string, object>)user;

            IdentityUser identityUser = await userManager.FindByIdAsync(id);
            var userAddress = await dbContext.Adresses.FindAsync(id);

            foreach (PropertyInfo prop in identityUser.GetType().GetProperties())
            {
                userDictionary.Add(prop.Name, prop.GetValue(identityUser, null));
            }

            foreach (PropertyInfo prop in userAddress.GetType().GetProperties())
            {
                userDictionary.Add(prop.Name, prop.GetValue(userAddress, null));
            }

            return userDictionary;
        }

        private async Task<string> Register(IdentityUser user, string password)
        {

            //user = new IdentityUser();
            //user.UserName = "TestUser";
            //user.Email = "TestUser@test.com";

            string massage = "";
            try
            {
                var checkUserExistance = await userManager.FindByNameAsync(user.UserName);
                if (checkUserExistance == null)
                {
                    var result = await userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                        massage = "OK";
                }
                else
                {
                    massage = "User not available";
                }

            }
            catch (Exception ex)
            {
                massage = ex.Message;
            }

            return await Task.FromResult(massage);
        }
    }
}



