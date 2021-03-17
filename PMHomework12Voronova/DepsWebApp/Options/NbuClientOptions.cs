using System;

namespace DepsWebApp.Options
{
    /// <summary>
    /// NBU client options.
    /// </summary>
    public class NbuClientOptions
    {
        /// <summary>
        /// Base address.
        /// </summary>
        public string BaseAddress { get; set; }

        /// <summary>
        /// Base address valid.
        /// </summary>
        public bool IsValid => !string.IsNullOrWhiteSpace(BaseAddress) &&
                               Uri.TryCreate(BaseAddress, UriKind.Absolute, out _);
    }
}
