using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Advanced.NET6.Utility.Filters
{
    /// <summary>
    /// 作用：（异步）缓存
    /// |
    /// 执行顺序：ing->控制器的构造函数->Actoin方法->ed
    /// </summary>
    public class CustomCacheAsyncResourceFilterAttribute : Attribute, IAsyncResourceFilter
    {
        


        private static Dictionary<string, object> CacheDictionary = new Dictionary<string, object>();
        /// <summary>
        /// 当XXX资源去执行的时候
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            Console.WriteLine("CustomCacheAsyncResourceFilterAttribute.OnResourceExecutionAsync.Before");

            string key = context.HttpContext.Request.Path;//请求的路径
            if (CacheDictionary.ContainsKey(key))
            {
                //只要是给Result赋值了，就会中断往后执行，直接返回给调用方
                context.Result = (IActionResult)CacheDictionary[key];
            }
            else
            {
                
                ResourceExecutedContext resource = await next.Invoke(); //这句话的执行就是去执行控制器的构造函数和Action方法
                CacheDictionary[key] = resource.Result;
                Console.WriteLine("CustomCacheAsyncResourceFilterAttribute.OnResourceExecutionAsync.After");
            }
            
        }
        

    }
}
