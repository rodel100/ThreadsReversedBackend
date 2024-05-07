using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models
{
    public class ThreadUser : IdentityUser
    {
        public string? ProfilePicture { get; set; }
    }
}
