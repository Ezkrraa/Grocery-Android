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
        private const string JwtStorageKey = "jwt_token";

        const string BaseUrl = "https://192.168.1.111:7020";
        public AuthController() { }

        public async Task<HttpResult> AttemptLogin(string username, string password)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            HttpClient client = new HttpClient(handler);
            client.Timeout = TimeSpan.FromSeconds(6);

            JsonContent content = JsonContent.Create(new Dictionary<string, string> { { "username", username }, { "password", password } }, new MediaTypeHeaderValue("application/json"));
            try
            {
                string url = BaseUrl + "/api/auth/login";
                HttpResponseMessage result = await client.PostAsync(url, content);
                if (result.IsSuccessStatusCode)
                    await SetToken(await result.Content.ReadAsStringAsync());
                handler.Dispose();
                client.Dispose();
                return result.IsSuccessStatusCode ? HttpResult.Success : HttpResult.AuthError;
            }
            // dns error etc.
            catch (HttpRequestException ex)
            {
                return HttpResult.ConnectionError;
            }
            // timeout
            catch (OperationCanceledException ex)
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
            string? token = await GetToken();
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

        public static async Task<string?> GetToken()
        {
            try
            {
                return await SecureStorage.Default.GetAsync(JwtStorageKey);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task SetToken(string token)
        {
            try
            {
                await SecureStorage.Default.SetAsync(JwtStorageKey, token);
            }
            catch (Exception ex)
            {
                return;
            }
        }
        public static void ClearToken()
        {
            SecureStorage.Remove(JwtStorageKey);
        }
    }
}
