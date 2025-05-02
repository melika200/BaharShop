using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaharShop.Application.Services.SignIn_v_Login
{
    //برای بازگشت نتیجه ورود کاربر
    //امل شناسه کاربر، نام کاربری و نقش‌های کاربر است 
    //زمانی که کاربر سعی می‌کند وارد سیستم شود، اطلاعاتی مانند ایمیل و رمز عبور را وارد می‌کند.پس از پردازش این اطلاعات، سیستم باید نتیجه ورود را به کاربر بازگرداند
    //این نتیجه می‌تواند
    // کاربر با موفقیت وارد سیستم شودیا نه
    public class ResultUserLoginDto
{
    public long UserId { get; set; }
    public string Username { get; set; }
}
}
