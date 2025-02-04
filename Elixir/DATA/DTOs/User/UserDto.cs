using Elixir.Entities;

namespace Elixir.DATA.DTOs.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public List<Guid> CategoriesId { get; set; }
        public int Followings { get; set; }
        public string Token { get; set; }
        public bool Active { get; set; }
        public Guid? StoreId { get; set; }
        public EmployeeRole? StoreRole { get; set; }
    }
}