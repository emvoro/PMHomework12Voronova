using System.ComponentModel.DataAnnotations;

namespace DepsWebApp.Models
{
    /// <summary>
    /// Authentication request body model.
    /// </summary>
    public class AuthRequest
    {
        /// <summary>
        /// Account login.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [MinLength(6)]
        public string Login { get; set; }

        /// <summary>
        /// Account password.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
