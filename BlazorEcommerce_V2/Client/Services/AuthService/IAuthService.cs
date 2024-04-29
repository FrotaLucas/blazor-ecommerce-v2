namespace BlazorEcommerce_V2.Client.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegister user);
    }
}
