using hidayah_collage.Models;
using hidayah_collage.Models.Paging;
using hidayah_collage.Models.Roles;
using System.Threading.Tasks;

namespace hidayah_collage.Interface
{
    public interface IRole
    {
        Task<WebResponse> GetAllRole();
        Task<WebResponse> AddRoleAsync(RoleRequest role);
        Task<WebResponse> DeleteRoleAsync(string roleName);
        Task<WebResponse> GetUserByRole(PagingRequest pagingRequest);
        Task<WebResponse> UserAssignment(UserAssignmentRequest userAssignmentRequest);
    }
}
