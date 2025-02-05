using AutoMapper;
using AutoMapper.QueryableExtensions;
using e_parliament.Interface;
using Elixir.DATA;
using Elixir.DATA.DTOs.User;
using Elixir.Entities;
using Elixir.Helpers;
using Elixir.Repository;
using Microsoft.EntityFrameworkCore;

namespace Elixir.Services
{
    public interface IUserService
    {
        Task<(UserDto? user, string? error)> Login(LoginForm loginForm);
        Task<(AppUser? user, string? error)> DeleteUser(Guid id);
        Task<(UserDto? UserDto, string? error)> Register(RegisterForm registerForm);
        Task<(UserDto? user, string? error)> UpdateUser(UpdateUserForm updateUserForm, Guid id);
        Task<(UserDto? user, string? error)> AddAdmin(AdminForm form, Guid id);

        Task<(UserDto? user, string? error)> AddUserCategory(UserCategoryForm form, Guid id);
        Task<(UserDto? user, string? error)> GetUserById(Guid id);
        Task<(List<UsersDto>? dtos, int? totalCount, string? error)> GetAll(UserFilter filter);

        Task<(string? token, string? error)> SwitchToStore(Guid StoreId, Guid UserId);
        Task<(string? token, string? error)> UserChangePassword(ChangePasswordForm form, Guid userId);

    }


    public class UserService : IUserService
    {
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public UserService(DataContext dbContext, IMapper mapper, ITokenService tokenService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<(UserDto? user, string? error)> Login(LoginForm loginForm)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == loginForm.UserName.ToLower().Trim() || u.PhoneNumber.Contains(loginForm.UserName.Trim()));

            if (user == null) return (null, "User not found");
            if (!BCrypt.Net.BCrypt.Verify(loginForm.Password, user.Password)) return (null, "Wrong password");

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Token = _tokenService.CreateToken(userDto);
            return (userDto, null);
        }

        public async Task<(AppUser? user, string? error)> DeleteUser(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(new object[] { id });
            if (user == null) return (null, "User not found");
            user.Deleted = true;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return (user, null);
        }

        public async Task<(UserDto? UserDto, string? error)> Register(RegisterForm registerForm)
        {
            var existingUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.UserName == registerForm.UserName.Trim() || u.PhoneNumber.Contains(registerForm.PhoneNumber.Trim()));
            if (existingUser != null) return (null, "User already exists");


            var newUser = new AppUser
            {
                UserName = registerForm.UserName.Trim(),
                FullName = registerForm.FullName.TrimEnd(),
                PhoneNumber = [
                    registerForm.PhoneNumber.Trim()
                ],
                Password = BCrypt.Net.BCrypt.HashPassword(registerForm.Password),
                Role = UserRole.User
            };

            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(newUser);
            userDto.Token = _tokenService.CreateToken(userDto);
            return (userDto, null);
        }

        public async Task<(UserDto? user, string? error)> UpdateUser(UpdateUserForm updateUserForm, Guid id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id && x.Deleted != true);
            if (user == null) return (null, "User not found");

            if (updateUserForm.UserName != null) user.UserName = updateUserForm.UserName.Trim();
            if (updateUserForm.FullName != null) user.FullName = updateUserForm.FullName.TrimEnd();
            if (updateUserForm.PhoneNumber != null) user.PhoneNumber = updateUserForm.PhoneNumber;



            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            var userDto = _mapper.Map<UserDto>(user);
            return (userDto, null);

        }

        public async Task<(UserDto? user, string? error)> GetUserById(Guid id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id && x.Deleted != true);
            if (user == null) return (null, "User not found");

            var userDto = _mapper.Map<UserDto>(user);
            return (userDto, null);
        }
        public async Task<(List<UsersDto>? dtos, int? totalCount, string? error)> GetAll(UserFilter filter)
        {
            var query = _dbContext.Users
            .AsNoTracking()
            .Where(x => !x.Deleted && x.Role != UserRole.SuperAdmin && (filter.Role == null || x.Role == filter.Role));
            var totalCount = await query.CountAsync();
            var dtos = await query
            .Paginate(filter)
                .ProjectTo<UsersDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return (dtos, totalCount, null);
        }

        public async Task<(UserDto? user, string? error)> AddUserCategory(UserCategoryForm form, Guid id)
        {
            var userExist = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (userExist == null) return (null, "user not found");

            var categories = await _dbContext.Categories.Where(x => form.CategoriesId.Contains(x.Id)).ToListAsync();
            if (categories == null) return (null, "categories are not found");
            var userCategoryList = new List<UserCategory>();
            foreach (var category in categories)
            {
                var userCategory = new UserCategory()
                {
                    UserId = userExist.Id,
                    CategoryId = category.Id
                };
                userCategoryList.Add(userCategory);
            }

            await _dbContext.UserCategories.AddRangeAsync(userCategoryList);
            await _dbContext.SaveChangesAsync();
            var userDto = _mapper.Map<UserDto>(userExist);
            return (userDto, null);
        }

        public async Task<(string? token, string? error)> SwitchToStore(Guid StoreId, Guid UserId)
        {
            var Store = await _dbContext.Stores.AnyAsync(x => x.Id == StoreId && !x.Deleted);
            if (!Store) return (null, "Store not found");

            var UserRoleInStore = await _dbContext.UserStores.Where(x => x.UserId == UserId && x.StoreId == StoreId)
            .Select(s => s.Role).FirstOrDefaultAsync();

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == UserId && !x.Deleted);
            var userDto = _mapper.Map<UserDto>(user);
            userDto.StoreId = StoreId;
            userDto.StoreRole = UserRoleInStore;
            var newToken = _tokenService.CreateToken(userDto);
            return (newToken, null);


        }

        public async Task<(string? token, string? error)> UserChangePassword(ChangePasswordForm form, Guid userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null) return (null, "User Not Found");
            if (!BCrypt.Net.BCrypt.Verify(form.OldPassword, user.Password)) return (null, "Wrong password");
            user.Password = BCrypt.Net.BCrypt.HashPassword(form.NewPassword);
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return ("Password Change Successfully", null);
        }

        public async Task<(UserDto? user, string? error)> AddAdmin(AdminForm form, Guid id)
        {
            var admin = await _dbContext.Users.FirstOrDefaultAsync(x=>x.Id == id);
            if(admin == null) return(null,"User not found");
            if(form.Role >= admin.Role) return (null,"you are not allowed");
            var newUser = new AppUser
            {
                UserName = form.UserName.Trim(),
                FullName = form.FullName.TrimEnd(),
                PhoneNumber = [
                    form.PhoneNumber.Trim()
                ],
                Password = BCrypt.Net.BCrypt.HashPassword(form.Password),
                Role = form.Role
            };

            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(newUser);
            return(userDto,null);
        }
    }
}