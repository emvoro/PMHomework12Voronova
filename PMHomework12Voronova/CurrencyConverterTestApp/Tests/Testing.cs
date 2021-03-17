using CurrencyConverterTestApp.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CurrencyConverterTestApp.Tests
{
    public class Testing
    {
        private readonly HttpClient _httpClient;

        public Testing(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private void Authentication(User user)
        {
            var bytes = Encoding.ASCII.GetBytes($"{user.Login}:{user.Password}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));
        }

        private async Task<HttpResponseMessage> RegistrationAsync(User user)
        {
            var requestContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync("auth/register", requestContent);
        }

        public async Task TestRegistration(int expectedCode, User user)
        {
            Console.WriteLine("\n Test Registration");

            var response = await RegistrationAsync(user);
            var content = await response.Content.ReadAsStringAsync();

            ConsoleWriteResponse("/register", new string(" Login                : " + user.Login + "\n Password             : " + user.Password), expectedCode, (int)response.StatusCode, content);
        }

        public async Task TestAuthorizedExchange(string fromCurrency, string toCurrency, int expectedCode, User user)
        {
            Console.WriteLine("\n Test Authorized Exchange");
            _httpClient.DefaultRequestHeaders.Clear();
            await RegistrationAsync(user);
            var request = $"rates/{fromCurrency}/{toCurrency}?amount=100";
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, request);
            httpRequest.Headers.Add("Authorization", Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Login + ":" + user.Password)));
            httpRequest.Content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(httpRequest);
            var content = await response.Content.ReadAsStringAsync();

            ConsoleWriteResponse(request, null, expectedCode, (int)response.StatusCode, content);
        }

        public async Task TestUnauthorizedExchange(string fromCurrency, string toCurrency, int expectedCode)
        {
            Console.WriteLine("\n Test Unauthorized Exchange");

            _httpClient.DefaultRequestHeaders.Clear();
            var request = $"rates/{fromCurrency}/{toCurrency}?amount=100";
            var response = await _httpClient.GetAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            ConsoleWriteResponse(request, null, expectedCode, (int)response.StatusCode, content);
        }

        public void ConsoleWriteResponse(string request, string body, int expectedStatusCode, int actualStatusCode, string content)
        {
            Console.WriteLine($"\n Request              : {request}");
            if(body != null) Console.WriteLine($"{body}");
            Console.WriteLine($" Expected Status Code : {expectedStatusCode}");
            Console.WriteLine($" Actual Status Code   : {actualStatusCode}");
            Console.WriteLine($" Response             : {content} ");

            WriteResult(expectedStatusCode == actualStatusCode);
        }

        public void WriteResult(bool isSucceeded)
        {
            Console.ForegroundColor = isSucceeded ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(isSucceeded ? " SUCCEEDED" : " FAILED");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
