
using Advanced.NET6.Utility.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NLog.Extensions.Logging;

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




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseSession();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();//鉴权

app.UseAuthorization();//授权

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
