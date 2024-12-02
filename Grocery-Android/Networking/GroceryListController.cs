using GroceryAndroid.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GroceryAndroid.Networking
{
    public class GroceryListController
    {
        public string BaseUrl = "https://192.168.1.111:7020";
        public GroceryListController() { }

        public async Task<CategoryListDTO?> GetItemsInCategory(Guid CategoryId)
        {
            HttpClient client = await GetClient();
            if (client == null)
                return null;
            try
            {
                string url = BaseUrl + "/api/item/by-category" + "?id=" + CategoryId.ToString();
                HttpResponseMessage result = await client.GetAsync(url);
                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<CategoryListDTO>(await result.Content.ReadAsStringAsync());
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<CategoryDisplayDTO>?> GetAllCategories()
        {
            HttpClient? client = await GetClient();
            if (client == null)
                return null;
            try
            {
                HttpResponseMessage result = await client.GetAsync(BaseUrl + "/api/category/all");
                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<IEnumerable<CategoryDisplayDTO>>(await result.Content.ReadAsStringAsync());
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static async Task<HttpClient?> GetClient()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            HttpClient client = new HttpClient(handler);
            client.Timeout = TimeSpan.FromSeconds(10);
            string? token = await AuthController.GetToken();
            if (token == null)
                return null;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }
    }
}
