using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using BaharShop.Common;

namespace BaharShop.Application.Services.SignIn_v_Login
{
    public interface IRegisterUserService
{
    ResultDto<ResultRegisterUserDto> Execute(RequestRegisterUserDto request);
        bool IsUserRegistered(string email);
    }

  
  

  
}
