using Elixir.Interface;

namespace Elixir.Repository
{
    public interface IRepositoryWrapper
    {
   
        IUserRepository User { get; }
        ICategoryRepository Category { get; }
        IUserAddressRepository UserAddress { get; }

        
    }
}