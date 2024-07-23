namespace BlazorEcommerce_V2.Client.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegister user);

        Task<ServiceResponse<string>> Login(UserLogin user);

        Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request);

        Task<bool> IsAuthenticated();
    }
}
