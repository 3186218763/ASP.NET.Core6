using Advanced.NET6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Advanced.NET6.Utility.Filters
{
    /// <summary>
    /// 作用：1.结果的统一处理 2.Json格式的统一处理
    /// |
    /// 执行顺序：构造函数->Action方法->ing->渲染视图->ed
    /// </summary>
    public class CustomResultFilterAttribute : Attribute, IResultFilter
    {
        /// <summary>
        /// 在 XX 结果之前
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnResultExecuting(ResultExecutingContext context)
        {
            Console.WriteLine("CustomResultFilterAttribute.OnResultExecuting");

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

        /// <summary>
        /// 在 XX 结果之后
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnResultExecuted(ResultExecutedContext context)
        {
            Console.WriteLine("CustomResultFilterAttribute.OnResultExecuted");

            
        }
    }
}
