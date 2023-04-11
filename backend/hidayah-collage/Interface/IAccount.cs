using hidayah_collage.Models;
using System.Threading.Tasks;

namespace hidayah_collage.Interface
{
    public interface IAccount
    {
        Task<WebResponse> Register(RegisterRequest registerRequest);
        Task<WebResponse> ConfirmEmailAsync(string userId, string token);
        Task<WebResponse> Login(LoginRequest loginRequest);
        Task<WebResponse> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest);
        Task<WebResponse> ResetPassword(ResetPasswordRequest resetPasswordRequest);
        Task<WebResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest);
        Task<WebResponse> Logout(string rawUserId);
        Task<WebResponse> GetUserInfo(string userId);
        Task<WebResponse> SendConfirmedEmail(string userId);
        Task<WebResponse> GetAllUser();
    }
}
