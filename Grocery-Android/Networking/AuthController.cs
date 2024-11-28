using Microsoft.Maui.Controls.PlatformConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GroceryAndroid.Networking
{
    public enum HttpResult
    {
        Success,
        ConnectionError,
        AuthError
    }


    public class AuthController
    {
        const string BaseUrl = "https://192.168.1.111:7020";
        public AuthController() { }

        public async Task<HttpResult> AttemptLogin(string username, string password)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            HttpClient client = new HttpClient(handler);

            JsonContent content = JsonContent.Create(new Dictionary<string, string> { { "username", username }, { "password", password } }, new MediaTypeHeaderValue("application/json"));
            try
            {
                string url = BaseUrl + "/api/auth/login";
                HttpResponseMessage result = await client.PostAsync(url, content);
                if (result.IsSuccessStatusCode)
                    await SecureStorage.SetAsync("jwt_token", await result.Content.ReadAsStringAsync());
                handler.Dispose();
                client.Dispose();
                return result.IsSuccessStatusCode ? HttpResult.Success : HttpResult.AuthError;
            }
#if DEBUG
            catch (HttpRequestException ex)
#else
            catch (HttpRequestException)
#endif
            {
                return HttpResult.ConnectionError;
            }
        }

        /// <summary>
        /// Checks if a valid JWT is set, to see if login should be skipped
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckToken()
        {
            string? token = await SecureStorage.GetAsync("jwt_token");
            if (token == null)
                return false;
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                HttpResponseMessage result = await client.PostAsync(BaseUrl + "/api/auth/check-token", null);

                handler.Dispose();
                client.Dispose();
                return result.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        public async Task<string?> GetToken()
        {
            return await SecureStorage.GetAsync("jwt_token");
        }
    }
}
