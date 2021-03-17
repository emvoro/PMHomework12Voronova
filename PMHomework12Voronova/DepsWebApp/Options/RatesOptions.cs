namespace DepsWebApp.Options
{
    /// <summary>
    /// Rates options.
    /// </summary>
    public class RatesOptions
    {
        /// <summary>
        /// Base currency.
        /// </summary>
        public string BaseCurrency { get; set; }

        /// <summary>
        /// Base currency valid.
        /// </summary>
        public bool IsValid => !string.IsNullOrWhiteSpace(BaseCurrency);
    }
}
