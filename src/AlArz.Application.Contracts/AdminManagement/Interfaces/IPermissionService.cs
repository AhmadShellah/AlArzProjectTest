using AlArz.Dtos;
using System.Threading.Tasks;

namespace AlArz.Interfaces
{
    public interface IPermissionService
    {
        //Task GrantRolePermissionDemoAsync(
        //    string roleName, string permission);

        //Task GrantUserPermissionDemoAsync(
        //    Guid userId, string roleName, string permission);

        //Task<IEnumerable<string>> GetAllForUserAsync(Guid userId);
        Task<UserInfo> GetAllForCurrentUserAsync(string userId);
    }
}
