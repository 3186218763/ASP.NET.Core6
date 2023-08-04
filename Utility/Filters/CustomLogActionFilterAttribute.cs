using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Advanced.NET6.Utility.Filters
{
    /// <summary>
    /// 作用：日志
    /// |
    /// 执行顺序：控制器的构造函数->ing->Action方法->ed
    /// </summary>
    public class CustomLogActionFilterAttribute : Attribute, IActionFilter
    {
        private readonly ILogger<CustomLogActionFilterAttribute> _Iogger;

        public CustomLogActionFilterAttribute(ILogger<CustomLogActionFilterAttribute> iogger)
        {
            _Iogger = iogger;
        }




        /// <summary>
        /// 在 XX 执行之前
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("CustomActionFilterAttribute.OnActionExecuting");
            var para = context.HttpContext.Request.QueryString.Value;
            var controllerName = context.HttpContext.GetRouteValue("controller");
            var actionName = context.HttpContext.GetRouteValue("action");
            _Iogger.LogInformation($"执行{controllerName}控制器--{actionName}方法; 参数为：{para}");
            
        }
        
        /// <summary>
        /// 在 XX 执行之后
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("CustomActionFilterAttribute.OnActionExecuted");

            var result = Newtonsoft.Json.JsonConvert.SerializeObject(context.Result);
            var controllerName = context.HttpContext.GetRouteValue("controller");
            var actionName = context.HttpContext.GetRouteValue("action");
            _Iogger.LogInformation($"执行{controllerName}控制器--{actionName}方法; 执行结果为：{result}");

        }
    }
}
