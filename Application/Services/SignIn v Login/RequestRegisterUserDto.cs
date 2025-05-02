using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaharShop.Application.Services.SignIn_v_Login
{
    //برای دریافت اطلاعات ثبت‌نام کاربر از فرم ثبت‌نام استفاده می‌شود.

    public class RequestRegisterUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
       
    }
}
