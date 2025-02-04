using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Elixir.Entities
{
    public class AppUser : BaseEntity<Guid>
    {
        public required string FullName { get; set; }

        public required string UserName { get; set; }

        public string? Email { get; set; }

        public required string Password { get; set; }

        [MinLength(1)]
        public required List<string> PhoneNumber { get; set; }

        public List<string>? Imgs { get; set; }

        public int Followings { get; set; }
        public int SavedProducts { get; set; }

        public UserRole Role { get; set; }
        public bool Active { get; set; }


        public List<UserStore>? UserStores { get; set; }
        public List<UserAddress>? UserAddresses { get; set; }
        public List<UserCategory>? Categories { get; set; }


        public void Following(int increase, int decrease)
        {
            Followings = Followings + increase - decrease;

        }

        public void SavedProduct(int increase, int decrease)
        {
            SavedProducts = SavedProducts + increase - decrease;

        }


    }
    public enum UserRole
    {
        User,
        Merchant,
        Admin,
        SuperAdmin
    }

}