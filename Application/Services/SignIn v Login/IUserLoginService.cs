using BaharShop.Common;
using Microsoft.EntityFrameworkCore;


namespace BaharShop.Application.Services.SignIn_v_Login
{
    public interface IUserLoginService
    {
        ResultDto<ResultUserLoginDto> Execute(string email, string password);
    }






}
