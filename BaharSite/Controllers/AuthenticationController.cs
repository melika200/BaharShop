using BaharShop.Application.Services.SignIn_v_Login;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace BaharSite.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IRegisterUserService _registerUserService;
        private readonly IUserLoginService _userLoginService;

        public AuthenticationController(IRegisterUserService registerUserService, IUserLoginService userLoginService)
        {
            _registerUserService = registerUserService;
            _userLoginService = userLoginService;
        }

      
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Register(RequestRegisterUserDto request)
        {
         
            if (!IsValidEmail(request.Email))
            {
                ViewBag.ErrorMessage = "ایمیل وارد شده معتبر نیست. لطفاً یک ایمیل معتبر وارد کنید.";
                return View(request);
            }

        
            if (request.Password != request.RePassword)
            {
                ViewBag.ErrorMessage = "رمز عبور و تکرار رمز عبور مطابقت ندارند. لطفاً رمز عبور را دوباره بررسی کنید.";
                return View(request);
            }


            var result = _registerUserService.Execute(request);
            if (result == null || !result.IsSuccess)
            {
                ViewBag.ErrorMessage = System.Web.HttpUtility.HtmlDecode(result?.Message ?? "نام کاربری یا ایمیل قبلاً ثبت شده است. لطفاً اطلاعات دیگری وارد کنید.");
                return View(request);
            }

            return await SignInUser(result.Data.UserId, request.Email, "Customer");
        }


        private bool IsValidEmail(string email)
        {
            return new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(email);
        }


      
        [HttpGet]
        public IActionResult Login(bool showRegisterAlert = false)
        {
            ViewBag.ShowRegisterAlert = showRegisterAlert;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var isRegistered = _registerUserService.IsUserRegistered(email); 

            if (!isRegistered)
            {
                ViewBag.ShowRegisterAlert = true;
                return View();
            }

            var result = _userLoginService.Execute(email, password);
            if (result.IsSuccess)
            {
                return await SignInUser(result.Data.UserId, email);
            }

            return View(result);
        }

            private async Task<IActionResult> SignInUser(long userId, string email, params string[] roles)
            {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Profile() => View();

        public async Task<IActionResult> SignOut()
        {
         
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
