
using Advanced.NET6.Utility.Filters;
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

// Add services to the container.
builder.Services.AddControllersWithViews(
/*
//全局注册，对整个项目都生效
MvcOptions => MvcOptions.Filters.Add<CustomCacheResourceFilterAttribute>()
*/
);

{
    builder.Services.AddSession();
}
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseSession();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
