using Azure.Core;
using BaharShop.Application.Interface.Context;
using BaharShop.Application.InterfaceRepository;
using BaharShop.Application.Services.SignIn_v_Login;
using BaharShop.Common;
using BaharShop.Domain;


namespace BaharShop.Application.Services.SignIn_v_Login
{
   // RegisterUserService: این کلاس مسئول ثبت‌نام کاربران است.در متد Execute، ابتدا بررسی می‌شود که آیا نام کاربری یا ایمیل قبلاً استفاده شده است یا خیر.سپس کاربر جدید ایجاد و به دیتابیس اضافه می‌شو
    public class RegisterUserService : IRegisterUserService
    {
        private readonly IDatabaseContext _dataBaseContext;
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher _passwordHasher;

      
        public RegisterUserService(IDatabaseContext dataBaseContext, IUserRepository userRepository)
        {
            _dataBaseContext = dataBaseContext;
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher();
        }

        public ResultDto<ResultRegisterUserDto> Execute(RequestRegisterUserDto request)
        {
            // بررسی صحت داده‌ها (مثلاً وجود نام کاربری یا ایمیل)
            if (_userRepository.Exists(request.Username, request.Email))
            {
                return new ResultDto<ResultRegisterUserDto>
                {
                    IsSuccess = false,
                    Message = "نام کاربری یا ایمیل قبلاً استفاده شده است."
                };
            }

            // ایجاد کاربر جدید
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PasswordHash = _passwordHasher.HashPassword(request.Password), // هش کردن رمز عبور
                CreatedAt = DateTime.UtcNow,
                Address = "آدرس ثبت نشده",
                Mobile="موبایلی ثبت نشده"
            };

            _userRepository.Add(user);
            _userRepository.SaveChanges(); // ذخیره تغییرات در دیتابیس

            return new ResultDto<ResultRegisterUserDto>
            {
                IsSuccess = true,
                Data = new ResultRegisterUserDto { UserId = user.Id }
            };
        }

        public bool IsUserRegistered(string email)
        {
            return _userRepository.GetByEmail(email) != null;
        }
    }

}