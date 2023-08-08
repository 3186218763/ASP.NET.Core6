
using Advanced.NET6.Utility.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NLog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

#region log4net
{
    /*
    //Nuget����
    //1.Log4Net
    //2.Microsoft.Extensions.Logging.Log4Net.AspNetCore
    builder.Logging.AddLog4Net("CfgFile/log4net.Config");
    */
}
#endregion

#region Nlogin
{
    //Nuget����
    builder.Logging.AddNLog("CfgFile/NLog.config");
}
#endregion

#region ʹ��Session
builder.Services.AddSession();
#endregion

// Add services to the container.
builder.Services.AddControllersWithViews(
/*
//ȫ��ע�ᣬ��������Ŀ����Ч
MvcOptions => MvcOptions.Filters.Add<CustomCacheResourceFilterAttribute>()
*/
);
#region ���ü�Ȩ
{
    //ѡ��ʹ�����ַ�ʽ����Ȩ
    builder.Services.AddAuthentication(option =>
    {
        option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        option.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        option.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, option =>
    {
        option.LoginPath = "/Fourth/Login";//���û���ҵ��û���Ϣ---��Ȩʧ��--��ȨҲʧ����---����ת��ָ����Action
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

app.UseAuthentication();//��Ȩ

app.UseAuthorization();//��Ȩ

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
