using System;
using System.Threading.Tasks;
using Learner.ViewModels;

namespace Learner.Interfaces
{
    public interface IIdentityService
    {
        Task SignIn(SignInViewModel model);
        Task LogOut();
        Task SignUp(SignUpViewModel model);
    }
}
