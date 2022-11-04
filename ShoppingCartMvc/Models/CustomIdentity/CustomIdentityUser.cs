using Microsoft.AspNetCore.Identity;

namespace ShoppingCartMvc.Models.CustomIdentity
{
    public class CustomIdentityUser:IdentityUser
    {
        public string NameSurname { get; set; }
        public bool IsActive { get; set; }
    }
}
