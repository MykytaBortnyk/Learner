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
        string uri;

        public IdentityService()
        {
            httpClient = App.httpClient;
            uri = App.Uri + "Account/";
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
        //чё? 
        public async Task<bool> SignIn(SignInViewModel model)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync(uri + "SignIn", model);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Success");
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }
            return false;
        }

        public async Task<bool> SignUp(SignUpViewModel model)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync(uri + "SignUp", model);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Success");
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }
            return false;
        }

        public async Task<bool> IsAuthenticated()
        {
            try
            {
                var response = await httpClient.GetAsync(uri + "IsAuthenticated");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Success");
                    var str = await response.Content.ReadAsStringAsync();
                    bool.TryParse(str, out bool result);
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }
            return false;
        }
    }
}
