using Advanced.NET6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Advanced.NET6.Utility.Filters
{
    /// <summary>
    /// 作用：（异步）1.结果的统一处理 2.Json格式的统一处理
    /// |
    /// 执行顺序：构造函数->Action方法->ing->渲染视图->ed
    /// </summary>
    public class CustomAsyncResultFilterAttribute : Attribute, IAsyncResultFilter
    {
        
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            {
                //渲染前

                if (context.Result is JsonResult)
                {
                    JsonResult result = (JsonResult)context.Result;
                    context.Result = new JsonResult(new AjaxResult()
                    {
                        Success = true,
                        Message = "OK",
                        Data = result.Value
                    });
                }
            }
            ResultExecutedContext executedContext = await next.Invoke();//这里去渲染结果

            {
                //渲染后
            }
        }
    }
}
