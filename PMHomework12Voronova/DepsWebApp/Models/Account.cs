using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepsWebApp.Models
{
    /// <summary>
    /// Account model.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Account Base64 identifier.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Account login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Account password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Account constructor.
        /// </summary>
        /// <param name="id">Set <see cref="Id"/>Account Base64 identifier.</param>
        /// <param name="login">Set <see cref="Login"/>Account login.</param>
        /// <param name="password">Set <see cref="Password"/>Account password.</param>
        public Account(string id, string login, string password)
        {
            Id = id;
            Login = login;
            Password = password;
        }
    }
}
