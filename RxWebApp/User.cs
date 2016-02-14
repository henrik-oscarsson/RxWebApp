using Microsoft.AspNet.Identity.EntityFramework;

namespace RxWebApp
{
    public class User : IdentityUser
    {
        public string DefaultLanguage { get; set; }
        public string DefaultCurrency { get; set; }
        public int? RetailerId { get; set; }
    }
}