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
            httpClient = App.httpClient;
            uri = new Uri(App.Uri, "Account");
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
        //TODO: SignIn and SignUp 
        public async Task SignIn(SignInViewModel model)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync(new Uri(uri, "SignIn"), model);

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

        public async Task SignUp(SignUpViewModel model)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync(new Uri(uri, "SignUp"), model);

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
    }
}
