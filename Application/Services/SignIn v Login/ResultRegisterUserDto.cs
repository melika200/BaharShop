using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaharShop.Application.Services.SignIn_v_Login
{
    //برای بازگشت نتیجه ثبت‌نام کاربر استفاده می‌شود
    //شامل شناسه کاربر جدید است 
    public class ResultRegisterUserDto
    {
        public long UserId { get; set; }
    }
}
