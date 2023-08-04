using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Advanced.NET6.Utility.Filters
{
    /// <summary>
    /// 作用：缓存
    /// |
    /// 执行顺序：ing->控制器的构造函数->Action方法->ed
    /// </summary>
    public class CustomCacheResourceFilterAttribute : Attribute, IResourceFilter
    {

        private static Dictionary<string, object> CacheDictionary = new Dictionary<string, object>();
        /// <summary>
        /// 在XX 之前
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            //匿名支持
            if (context.ActionDescriptor.EndpointMetadata.Any(c => c.GetType().Equals(typeof(AllowAnonymousAttribute))))
            {
                return;
            }

            string key = context.HttpContext.Request.Path;
            if (CacheDictionary.ContainsKey(key))
            {
                context.Result = (IActionResult)CacheDictionary[key];
            }
            Console.WriteLine("CustomCacheResourceFilterAttribute.OnResourceExecuting");
        }

        /// <summary>
        /// 在XX 之后
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            //匿名支持
            if (context.ActionDescriptor.EndpointMetadata.Any(c => c.GetType().Equals(typeof(AllowAnonymousAttribute))))
            {
                return;
            }

            string key = context.HttpContext.Request.Path;
            CacheDictionary[key] = context.Result;
            Console.WriteLine("CustomCacheResourceFilterAttribute.OnResourceExecuted");
        }
    }
}
