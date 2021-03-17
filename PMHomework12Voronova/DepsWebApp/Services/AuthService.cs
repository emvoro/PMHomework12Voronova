using DepsWebApp.Converters;
using DepsWebApp.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DepsWebApp.Services
{
    /// <summary>
    /// Authentication service for registration.
    /// </summary>
    public class AuthService : IAuthService
    {
        private static readonly List<Account> _accounts = new List<Account>();

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        /// <inheritdoc/>
        public async Task<string> RegisterAsync(string login, string password)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));
            if (password == null) throw new ArgumentNullException(nameof(password));

            if (_accounts.Any(account => account.Login == login)) return null;

            password = GetHashString(password);
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(login + ":" + password));

            await _semaphore.WaitAsync();
            _accounts.Add(new Account(base64, login, password));
            _semaphore.Release();

            return base64;
        }

        /// <inheritdoc/>
        public async Task<string> GetUserAccount(string accountInBase64)
        {
            if (accountInBase64 == null) throw new ArgumentNullException(nameof(accountInBase64));

            var splitted = accountInBase64.Split(':');
            var login = splitted[0];
            var password = splitted[1];

            await _semaphore.WaitAsync();
            var account = _accounts.FirstOrDefault(x => x.Login == login && x.Password == password);
            _semaphore.Release();

            return account.Id;
        }

        private static string GetHashString(string str)
        {
            var bytes = Encoding.Unicode.GetBytes(str);
            var CSP = new MD5CryptoServiceProvider();
            var byteHash = CSP.ComputeHash(bytes);
            var hash = string.Empty;

            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return hash;
        }
    }
}
