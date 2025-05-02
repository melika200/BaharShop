using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaharShop.Application.Interface.Context;
using BaharShop.Application.InterfaceRepository;
using BaharShop.Common;
using Microsoft.EntityFrameworkCore;


namespace BaharShop.Application.Services.SignIn_v_Login
{
   // UserLoginService: این کلاس مسئول پردازش ورود کاربران است.در متد Execute، ابتدا کاربر با ایمیل جستجو می‌شود و سپس رمز عبور بررسی می‌شود.اگر ورود موفق باشد، نقش‌های کاربر نیز بارگذاری می‌شوند.


    public class UserLoginService : IUserLoginService
    {
        private readonly IDatabaseContext _dataBaseContext;
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher _passwordHasher;

        public UserLoginService(IDatabaseContext dataBaseContext, IUserRepository userRepository)
        {
            _dataBaseContext = dataBaseContext;
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher();
        }

        public ResultDto<ResultUserLoginDto> Execute(string email, string password)
        {
            var user = _userRepository.GetByEmail(email);
            if (user == null || !_passwordHasher.VerifyPassword(user.PasswordHash, password)) // تأیید رمز عبور
            {
                return new ResultDto<ResultUserLoginDto>
                {
                    IsSuccess = false,
                    Message = "نام کاربری یا رمز عبور نادرست است."
                };
            }

        

            return new ResultDto<ResultUserLoginDto>
            {
                IsSuccess = true,
                Data = new ResultUserLoginDto
                {
                    UserId = user.Id,
                    Username = user.Username,
                 
                }
            };
        }
    }

}

