
using AutoMapper;
using Elixir.DATA;
using Elixir.Entities;
using Elixir.Interface;
using Elixir.Respository;

namespace Elixir.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        private IUserRepository _user;  
        private ICategoryRepository _category;
        private IUserAddressRepository _userAddress;

        


        public ICategoryRepository Category {  get {
            if(_category == null)
            {
                _category = new CategoryRepository(_context,_mapper);
            }
            return _category;
        } }

        public IUserAddressRepository UserAddress {  get {
            if(_userAddress == null)
            {
                _userAddress = new UserAddressRepository(_context,_mapper);
            }
            return _userAddress;
        } }
        
        
        public IUserRepository User {  get {
            if(_user == null)
            {
                _user = new UserRepository(_context,_mapper);
            }
            return _user;
        } }

       

        public RepositoryWrapper(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
    }
}