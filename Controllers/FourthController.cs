using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Advanced.NET6.Controllers
{
    public class FourthController : Controller
    {

        /// <summary>
        /// 如果这里是一个数据列表
        /// 部分人能看---部分人是不能看
        /// </summary>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
        public IActionResult Index()
        {
            var user = HttpContext.User;
            return View();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "User")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Teach")]
        public IActionResult Index1()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin, User")]
        public IActionResult Index2()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Policy = "rolePolicy")]
        public IActionResult Index3()
        {
            return View();
        }

        /// <summary>
        /// 登录页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult LOGIN()
        {
            return View();
        }


        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(string name, string password)
        {
            if ("Richard".Equals(name) && "1".Equals(password))
            {
                var claims = new List<Claim>()//鉴别你是谁，相关信息
                {
                    new Claim("Userid","1"),
                    new Claim(ClaimTypes.Role,"Admin"),
                    new Claim(ClaimTypes.Role,"User"),
                    new Claim(ClaimTypes.Name,$"{name}--来自于Cookies"),
                    new Claim(ClaimTypes.Email,$"18672713698@163.com"),
                    new Claim("password",password),//可以写入任意数据
                    new Claim("Account","Administrator"),
                    new Claim("role","admin"),
                    new Claim("QQ","1030499676")
                };
                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Customer"));
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30),//过期时间：30分钟

                }).Wait();
                var user = HttpContext.User;
                return base.Redirect("/Fourth/Index");
            }
            else
            {
                base.ViewBag.Msg = "用户或密码错误";
            }
            return await Task.FromResult<IActionResult>(View());
        }
    }
}
