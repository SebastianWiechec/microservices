using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace UserApi.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<IdentityUser>> GetUsers();
        Task<IDictionary<string, object>> GetUserById(string id);
        Task<string> AddUser(IdentityUser user, string password);
        Task<ExpandoObject> UpdateUser(ExpandoObject user);
        Task<string> DeleteUser(string id);
    }
}
