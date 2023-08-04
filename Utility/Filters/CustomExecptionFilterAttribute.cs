using Microsoft.AspNetCore.Mvc.Filters;

namespace Advanced.NET6.Utility.Filters
{
    /// <summary>
    /// 功能：处理异常
    /// </summary>
    public class CustomExecptionFilterAttribute : Attribute, IExceptionFilter, IAsyncExceptionFilter
    {
        /// <summary>
        /// 当有异常发生，就会触发这里
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnException(ExceptionContext context)
        {
            Console.WriteLine("CustomExecptionFilterAttribute.OnException");
        }

        /// <summary>
        /// （异步版本）当有异常发生，就会触发这里
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task OnExceptionAsync(ExceptionContext context)
        {
            Console.WriteLine("CustomExecptionFilterAttribute.OnExceptionAsync");
            throw new NotImplementedException();
        }
    }
}
