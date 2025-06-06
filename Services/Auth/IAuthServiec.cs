
using EcommerceApi.Models.User;

namespace AuthenticationApp.Services
{
    public interface IAuthServiec
    {
        public Task<AuthModel> Registeration(RegisteredModel registeredModel);
        public Task<AuthModel> SignIn(SignInModel model);
        public Task<string> AddRole(RoleModel model);
        Task<AuthModel> RefrachToken(string token);
    }
}
