namespace B2CUserManager.Models
{
    public class AzureB2CSettings
    {
        public string? Tenant { get; set; }
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public string? Scopes { get; set; }
    }
}
