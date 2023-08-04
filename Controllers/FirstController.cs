using Microsoft.AspNetCore.Mvc;

namespace Advanced.NET6.Controllers
{
    public class FirstController : Controller
    {
        /// <summary>
        /// MVC
        /// C:业务逻辑计算-调用其他的服务做业务逻辑计算
        /// M:model 实体对象，保持数据，数据传输
        /// V:Views 视图---表现层
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            {
                //有一对业务逻辑计算完后
            }
            ViewBag.User1 = "张三";
            ViewData["User2"] = "李四";
            TempData["User3"] = "王五";
            HttpContext.Session.SetString("User4", "赵六");
            object User5 = "田七";
            return View(User5);
        }
    }
}
