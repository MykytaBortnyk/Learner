using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RestClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            RestService<WordViewModel> rest = new RestService<WordViewModel>();

            SignInViewModel userViewModel = new SignInViewModel
            {
                Email = "user@example.com",
                Password = "string",
                RememberMe = true
            };

            try
            {
                await rest.httpClient.GetAsync("https://localhost:5001/api/Account/SignOut");
                //var response = await rest.httpClient.PostAsync("https://localhost:5001/api/Account/SignIn", content);
                var response = await rest.httpClient.PostAsJsonAsync("https://localhost:5001/api/Account/SignIn", userViewModel);

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

    public class SignInViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class WordViewModel
    {
        /// <summary>
        /// Слово by itself
        /// </summary>
        public String Text { get; set; }

        /// <summary>
        /// Временный вариант трнаскрипции, будет зависеть от языка
        /// </summary>
        public String Transcription { get; set; }

        /// <summary>
        /// Перевод слова
        /// </summary>
        public String Translation { get; set; }

        /// <summary>
        /// Временный вариант записи языка, в будущем будет реализован отдельной моделью
        /// </summary>
        public String Language { get; set; }

        /// <summary>
        /// Навигационное свойство
        /// </summary>
        public List<Collection> Collections { get; set; }
    }
    public class Collection
    {

    }

    public class RestService<T> where T : class
    {
        public readonly HttpClient httpClient;

        string uri = "localhost:5001/api/";

        public RestService()
        {
            httpClient = new HttpClient();
        }

        public async Task<List<T>> GetAsync()
        {
            HttpResponseMessage response;
            try
            {
                response = await httpClient.GetAsync(uri + typeof(T));
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<T>>();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            HttpResponseMessage response;
            try
            {
                response = await httpClient.GetAsync(uri + typeof(T) + "/" + id);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<T>();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public async Task PostAsync(T item)
        {
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await httpClient.PostAsJsonAsync(uri + nameof(T), item);

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

        public async Task Put(T item)
        {
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await httpClient.PutAsJsonAsync(uri + nameof(T), content);
                var response1 = await httpClient.PutAsJsonAsync(uri + nameof(T), item);

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

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var response = await httpClient.DeleteAsync(uri + typeof(T) + "/" + id);
                //???
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
