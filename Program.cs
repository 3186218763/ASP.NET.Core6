
using Advanced.NET6.Utility.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

#region log4net
{
    /*
    //Nuget引入
    //1.Log4Net
    //2.Microsoft.Extensions.Logging.Log4Net.AspNetCore
    builder.Logging.AddLog4Net("CfgFile/log4net.Config");
    */
}
#endregion

#region Nlogin
{
    //Nuget引入
    builder.Logging.AddNLog("CfgFile/NLog.config");
}
#endregion



#region 使用Session
builder.Services.AddSession();
#endregion

// Add services to the container.
builder.Services.AddControllersWithViews(
/*
//全局注册，对整个项目都生效
MvcOptions => MvcOptions.Filters.Add<CustomCacheResourceFilterAttribute>()
*/
);

#region 配置鉴权
{
    //选择使用那种方式来鉴权
    builder.Services.AddAuthentication(option =>
    {
        option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        option.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        option.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, option =>
    {
        option.LoginPath = "/Fourth/Login";//如果没有找到用户信息---鉴权失败--授权也失败了---就跳转到指定的Action
        option.AccessDeniedPath = "/Home/NoAuthority";
    });
}
#endregion

#region 策略授权

{
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("rolePolicy", policyBuilder =>
        {
            /*
            policyBuilder.RequireRole("Teacher");
            policyBuilder.RequireClaim("Account");//必须包含某一个Claim
            */

            policyBuilder.RequireAssertion(context =>
            {
                bool bResult = context.User.HasClaim(c => c.Type == ClaimTypes.Role)
                               && context.User.Claims.First(c => c.Type.Equals(ClaimTypes.Role)).Value == "Admin"
                               && context.User.Claims.Any(c => c.Type == ClaimTypes.Name);

                //UserService userService = new UserService();
                ////userService.Validata(); 
                return bResult;

                
            });
        });
    });
    
}

#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

#region 中间件处理异常
{
    ///如果Http请求中的Response中的状态不是200,就会进入Home/Error中；
    app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");//只要不是200 都能进来

    //下面这个是自己拼装一个Reponse 输出
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync("<html lang=\"en\"><body>\r\n");
            await context.Response.WriteAsync("ERROR!<br><br>\r\n");
            var exceptionHandlerPathFeature =
                context.Features.Get<IExceptionHandlerPathFeature>();

            Console.WriteLine("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
            Console.WriteLine($"{exceptionHandlerPathFeature?.Error.Message}");
            Console.WriteLine("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");

            if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
            {
                await context.Response.WriteAsync("File error thrown!<br><br>\r\n");
            }
            await context.Response.WriteAsync("<a href=\"/\">Home</a><br>\r\n");
            await context.Response.WriteAsync("</body></html>\r\n");
            await context.Response.WriteAsync(new string(' ', 512)); // IE padding
        });
    });
}
#endregion

app.UseSession();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();//鉴权

app.UseAuthorization();//授权

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
