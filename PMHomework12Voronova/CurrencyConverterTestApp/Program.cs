using CurrencyConverterTestApp.Models;
using CurrencyConverterTestApp.Tests;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CurrencyConverterTestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            try
            {
                httpClient.BaseAddress = new Uri(JsonSerializer.Deserialize<Address>(File.ReadAllText("Settings/settings.json")).BaseAddress);
            }
            catch(Exception ex) when (ex is FileNotFoundException || ex is JsonException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Oops! Settings file not found or corrupted.\n " + ex.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            var testing = new Testing(httpClient);

            try
            {
                Console.WriteLine("\n Test Application by Emilia Voronova.");

                await testing.TestRegistration(200, new User("string", "string"));

                await testing.TestRegistration(400, new User("str", "str"));

                await testing.TestRegistration(409, new User("string", "string"));

                await testing.TestAuthorizedExchange("USD", "UAH", 200, new User("strifghfhng", "strhjhkhkhin"));

                await testing.TestUnauthorizedExchange("USD", "UAH", 401);

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Oops! Something went wrong.\n " + ex.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}
