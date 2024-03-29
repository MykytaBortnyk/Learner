﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Learner.Interfaces;
using Xamarin.Essentials;

namespace Learner.Services
{
    public class RestService<T> : IRestService<T> where T : class
    {
        private readonly HttpClient httpClient;
        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
        string uri;

        public RestService()
        {
            httpClient = App.httpClient;
            //httpClient.BaseAddress = new Uri(App.Uri);
            uri = App.Uri + typeof(T).Name;
        }

        public async Task<List<T>> GetAsync()
        {
            HttpResponseMessage response;
            try
            {
                response = await httpClient.GetAsync(uri);
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
                response = await httpClient.GetAsync(uri + id.ToString());
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
            try
            {
                var response = await httpClient.PostAsJsonAsync(uri, item);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Post sent successfully");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }
        }

        public async Task PutMany(List<T> items)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync(uri + "/Sync", items);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Put sent successfully");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }
        }

        public async Task Put(T item)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync(uri, item);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Put sent successfully");
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
                var response = await httpClient.DeleteAsync(uri + id.ToString());
                //???
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
