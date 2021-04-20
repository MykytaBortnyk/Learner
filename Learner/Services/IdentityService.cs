using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Learner.Interfaces;
using Learner.ViewModels;

namespace Learner.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient httpClient;
        Uri uri;

        public IdentityService()
        {
            uri = new Uri(String.Format(App.Uri, "Account"));
        }

        public async Task LogOut()
        {
            try
            {
                var response = await httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Success");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }
        }

        public Task SignIn(SignInViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task SignUp(SignUpViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
