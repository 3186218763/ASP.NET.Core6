using Advanced.NET6.Utility.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Advanced.NET6.Controllers
{
    //[CustomCacheResourceFilter]  : 这样可以对控制器的全部Action方法生效
    public class ThirdController : Controller
    {

        private readonly ILogger<SecondController> _Logger;
        public ThirdController(ILogger<SecondController> logger)
        {
            this._Logger = logger;
            this._Logger.LogInformation($"{this.GetType().Name} 被构造了。。");
        }
        #region ResourceFilter
        [CustomCacheResourceFilter]
        public IActionResult Index()
        {
            //1.定义一个缓存的区域
            //2请求来了，根据缓存的标识--判断缓存如果有缓存，就返回缓存的值
            //3.如果没有缓存--做计算
            //4.计算结果保存到缓存中去


            {
                //这里有业务逻辑-一调用业务逻辑计算的结果
            }

            ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd  ss");
            Console.WriteLine("这里是 Index被执行的方法");
            return View();
        }

        [CustomCacheAsyncResourceFilter]
        public IActionResult Index1()
        {
            {
                //支持缓存
            }
            ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd  ss");
            Console.WriteLine("这里是Index1 方法被执行");
            return View();
        }
        #endregion

        #region ActionFilter
        /*[CustomLogActionFilter]*/ //IOC容器的问题
        [TypeFilter(typeof(CustomLogActionFilterAttribute))]
        public IActionResult Index2(int id)
        {
            ViewBag.user = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                Id = id,
                Name="song -- ViewBag",
                Age=34
            });

            ViewData["UserInfo"] = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                Id = id,
                Name = "song -- ViewData",
                Age = 34
            });
            object description = "welcome you";

            return View(description);
        }
        #endregion

        #region ResultFilter

        [CustomResultFilter]
        public IActionResult Index3()
        {
            return View();
        }

        [CustomResultFilter]
        public IActionResult Index4()
        {
            return Json(new
            {
                Id = 123,
                Name = "song",
                Age = 20
            });
        }
        #endregion

        #region CustomAlwaysRunResultFilter
        [CustomAlwaysRunResultFilter]
        public IActionResult Index5()
        {
            return Json(new { 
                Id = 23,
                Name = "song",
                Age = 20,
            });
        }
        #endregion

        #region 匿名支持

        /// <summary>
        /// AllowAnonymous 不能直接匿名支持IResource,IResult,IAction。需要对后面3个进行扩展
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Index6()
        {


            return Json(new
            {
                Id = 123,
                Name = "song",
                Age = 20
            });
        }
        #endregion

        #region ExceptionFilter

        [CustomExecptionFilter]
        public IActionResult Index7()
        {
            throw new Exception("其实没有什么，就是测试");
        }
        #endregion
    }
}
