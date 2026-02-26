using AutoMapper;
using Entities.Dtos;
using Microsoft.AspNetCore.Identity;
using Services.Contracts;

namespace Services
{
    /// <summary>
    /// Kullanýcý kimlik doðrulama ve yetkilendirme iþlemlerini yöneten servis sýnýfýdýr.
    /// </summary>
    public class AuthManager : IAuthService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        /// <summary>
        /// AuthManager sýnýfýnýn yapýcý metodudur.
        /// </summary>
        /// <param name="roleManager">Rol yönetimi servisi.</param>
        /// <param name="userManager">Kullanýcý yönetimi servisi.</param>
        /// <param name="mapper">Nesne eþleme (mapping) servisi.</param>
        public AuthManager(RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
            IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Sistemdeki tüm rollerin listesini döndürür.
        /// </summary>
        public IEnumerable<IdentityRole> Roles =>
            _roleManager.Roles;

        /// <summary>
        /// Yeni bir kullanýcý oluþturur ve isteðe baðlý olarak rollerini tanýmlar.
        /// </summary>
        /// <param name="userDto">Kullanýcý oluþturma bilgileri DTO'su.</param>
        /// <returns>Kullanýcý oluþturma sonucunu döner.</returns>
        public async Task<IdentityResult> CreateUser(UserDtoForCreation userDto)
        {
            var user = _mapper.Map<IdentityUser>(userDto);
            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
                throw new Exception("User could not be created.");

            if (userDto.Roles.Count > 0)
            {
                var roleResult = await _userManager.AddToRolesAsync(user, userDto.Roles);
                if (!roleResult.Succeeded)
                    throw new Exception("System have problems with roles.");
            }

            return result;
        }

        /// <summary>
        /// Belirtilen kullanýcý adýndaki kullanýcýyý sistemden siler.
        /// </summary>
        /// <param name="userName">Silinecek kullanýcýnýn kullanýcý adý.</param>
        /// <returns>Silme iþleminin sonucunu döner.</returns>
        public async Task<IdentityResult> DeleteOneUser(string userName)
        {
            var user = await GetOneUser(userName);
            return await _userManager.DeleteAsync(user);
        }

        /// <summary>
        /// Sistemdeki tüm kullanýcýlarý listeler.
        /// </summary>
        /// <returns>Kullanýcý listesi.</returns>
        public IEnumerable<IdentityUser> GetAllUsers()
        {
            return _userManager.Users.ToList();
        }

        /// <summary>
        /// Belirtilen kullanýcý adýna sahip kullanýcýyý döner.
        /// </summary>
        /// <param name="userName">Kullanýcý adý.</param>
        /// <returns>Ýlgili kullanýcý nesnesi.</returns>
        public async Task<IdentityUser> GetOneUser(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is not null)
                return user;
            throw new Exception("User could not be found.");
        }

        /// <summary>
        /// Kullanýcýyý güncellemek için gerekli verileri içeren DTO'yu döner.
        /// </summary>
        /// <param name="userName">Güncellenecek kullanýcýnýn adý.</param>
        /// <returns>Kullanýcý DTO'su (roller dahil).</returns>
        public async Task<UserDtoForUpdate> GetOneUserForUpdate(string userName)
        {
            var user = await GetOneUser(userName);
            var userDto = _mapper.Map<UserDtoForUpdate>(user);
            userDto.Roles = new HashSet<string>(Roles.Select(r => r.Name).ToList());
            userDto.UserRoles = new HashSet<string>(await _userManager.GetRolesAsync(user));
            return userDto;
        }

        /// <summary>
        /// Bir kullanýcýnýn parolasýný sýfýrlar.
        /// </summary>
        /// <param name="model">Parola sýfýrlama bilgileri DTO'su.</param>
        /// <returns>Parola sýfýrlama sonucunu döner.</returns>
        public async Task<IdentityResult> ResetPassword(ResetPasswordDto model)
        {
            var user = await GetOneUser(model.UserName);
            await _userManager.RemovePasswordAsync(user);
            var result = await _userManager.AddPasswordAsync(user, model.Password);
            return result;
        }

        /// <summary>
        /// Belirtilen kullanýcýya ait bilgileri ve rollerini günceller.
        /// </summary>
        /// <param name="userDto">Güncelleme bilgileri içeren DTO.</param>
        public async Task Update(UserDtoForUpdate userDto)
        {
            var user = await GetOneUser(userDto.UserName);
            user.PhoneNumber = userDto.PhoneNumber;
            user.Email = userDto.Email;
            var result = await _userManager.UpdateAsync(user);
            if (userDto.Roles.Count > 0)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var r1 = await _userManager.RemoveFromRolesAsync(user, userRoles);
                var r2 = await _userManager.AddToRolesAsync(user, userDto.Roles);
            }
            return;
        }
    }
}
