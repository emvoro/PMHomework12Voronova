using DepsWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepsWebApp.Services
{
    /// <summary>
    /// Account service abstraction.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers user and assigns unique account id.
        /// </summary>
        /// <param name="login">User login.</param>
        /// <param name="password">User password.</param>
        /// <returns>Returns account of the created user or <c>null</c> if login already existed.</returns>
        /// <exception cref="ArgumentNullException">Throws when one of the arguments is null.</exception>
        Task<string> RegisterAsync(string login, string password);

        /// <summary>
        /// Gets user by encrypted string.
        /// </summary>
        /// <param name="accountInBase64">Encrypted user.</param>
        /// <returns>Returns account of the created user from encrypted string.</returns>
        Task<string> GetUserAccount(string accountInBase64);
    }
}
