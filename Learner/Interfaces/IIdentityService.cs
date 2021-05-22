using System;
using System.Threading.Tasks;
using Learner.ViewModels;

namespace Learner.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> SignIn(SignInViewModel model);
        Task LogOut();
        Task<bool> SignUp(SignUpViewModel model);
    }
}