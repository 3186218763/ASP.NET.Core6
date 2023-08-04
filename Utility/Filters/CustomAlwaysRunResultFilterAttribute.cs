using Microsoft.AspNetCore.Mvc.Filters;

namespace Advanced.NET6.Utility.Filters
{
    /// <summary>
    /// 特点：Result返回后，也不好中断
    /// |
    /// 执行顺序：构造函数->Action方法->ing->渲染视图->ed
    /// </summary>
    public class CustomAlwaysRunResultFilterAttribute : Attribute, IAlwaysRunResultFilter
    {

        public void OnResultExecuting(ResultExecutingContext context)
        {
            Console.WriteLine("CustomAlwaysRunResultFilterAttribute.OnResultExecuting");
        }
        public void OnResultExecuted(ResultExecutedContext context)
        {
            Console.WriteLine("CustomAlwaysRunResultFilterAttribute.OnResultExecuted");
        }
    }
}
