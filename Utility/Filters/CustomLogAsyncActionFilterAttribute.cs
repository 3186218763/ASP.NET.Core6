using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Advanced.NET6.Utility.Filters
{
    /// <summary>
    /// 作用：日志
    /// |
    /// 执行顺序：控制器的构造函数->ing->Action方法->ed
    /// </summary>
    public class CustomLogAsyncActionFilterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly ILogger<CustomLogAsyncActionFilterAttribute> _Iogger;

        public CustomLogAsyncActionFilterAttribute(ILogger<CustomLogAsyncActionFilterAttribute> iogger)
        {
            _Iogger = iogger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine("CustomLogAsyncActionFilterAttribute.OnActionExecutionAsync");

            var para = context.HttpContext.Request.QueryString.Value;
            var controllerName = context.HttpContext.GetRouteValue("controller");
            var actionName = context.HttpContext.GetRouteValue("action");
            _Iogger.LogInformation($"执行{controllerName}控制器--{actionName}方法; 参数为：{para}");

            ActionExecutedContext executedContext = await next.Invoke(); //这句话就是去执行Action

            var result = Newtonsoft.Json.JsonConvert.SerializeObject(context.Result);
            _Iogger.LogInformation($"执行{controllerName}控制器--{actionName}方法; 执行结果为：{result}");
        }
    }
}
