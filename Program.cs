
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

#region ������Ȩ

{
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("rolePolicy", policyBuilder =>
        {
            /*
            policyBuilder.RequireRole("Teacher");
            policyBuilder.RequireClaim("Account");//�������ĳһ��Claim
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

#region �м�������쳣
{
    ///���Http�����е�Response�е�״̬����200,�ͻ����Home/Error�У�
    app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");//ֻҪ����200 ���ܽ���

    //����������Լ�ƴװһ��Reponse ���
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

app.UseAuthentication();//��Ȩ

app.UseAuthorization();//��Ȩ

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
