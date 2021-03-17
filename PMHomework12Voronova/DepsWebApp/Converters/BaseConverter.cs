using System;
using System.Text;

namespace DepsWebApp.Converters
{
    public class BaseConverter
    {
        private static readonly string _encodingName = "ISO-8859-1";

        /// <summary>
        /// Encode string to base64 string
        /// </summary>
        /// <param name="str">Default string</param>
        /// <returns>Encoded string</returns>
        public static string ToBase64String(string str)
        {
            return "Basic " + Convert.ToBase64String(Encoding.GetEncoding(_encodingName).GetBytes(str));
        }

        /// <summary>
        /// Decode string from base64 string.
        /// </summary>
        /// <param name="str">Base64 string.</param>
        /// <returns>Decoded string.</returns>
        public static string FromBase64String(string str)
        {
            return Encoding.GetEncoding(_encodingName).GetString(
                    Convert.FromBase64String(
                        str.ToString().Split("Basic ")[1]));
        }
    }
}
