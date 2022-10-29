namespace NMRAPIs.Core.Constants
{
    /// <summary>
    /// Defines the application settings.
    /// </summary>
    public class ApplicationSettings
    {
    }

    /// <summary>
    /// Defines the AzureAd settings.
    /// </summary>
    public class AzureAdSettings
    {
        /// <summary>
        /// Gets or sets the TenantId.
        /// </summary>
        public string TenantId { get; set; }
    }

    public static class CommonSettings
    {
        public static ApplicationSettings AppSettings { get; set; }
    }
}
