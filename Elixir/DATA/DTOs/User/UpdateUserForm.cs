namespace Elixir.DATA.DTOs.User
{
    public class UpdateUserForm
    {
        public string? FullName { get; set; }

        public string? UserName { get; set; }

        public List<string>? PhoneNumber { get; set; }
        

    }
}