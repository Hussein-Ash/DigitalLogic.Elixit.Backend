using System;
using AutoMapper;
using Elixir.DATA;
using Elixir.Entities;
using Elixir.Interface;
using Elixir.Repository;

namespace Elixir.Respository;

public class UserAddressRepository : GenericRepository<UserAddress, Guid>, IUserAddressRepository
{
    public UserAddressRepository(DataContext context, IMapper mapper) : base(context, mapper)
    {
    }
}
