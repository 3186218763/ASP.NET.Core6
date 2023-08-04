using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Advanced.NET6.Controllers
{
    public class SecondController : Controller
    {
        private readonly ILogger<SecondController> _Logger;
        private readonly ILoggerFactory _LoggerFactory;
        public SecondController(ILogger<SecondController> logger, ILoggerFactory loggerFactory)
        {
            this._Logger = logger;
            this._Logger.LogInformation($"{this.GetType().Name} 被构造了。。");
            this._LoggerFactory = loggerFactory;
            ILogger<SecondController> _Logger2= this._LoggerFactory.CreateLogger<SecondController>();
            _Logger2.LogInformation($"{this.GetType().Name} 被构造了。。_Logger2");
        }
        public IActionResult Index()
        {
            ILogger<SecondController> _Logger3 = this._LoggerFactory.CreateLogger<SecondController>();
            _Logger3.LogInformation($"{this.GetType().Name} 被构造了。。_Logger3");

            return View();
        }
        public IActionResult Level() 
        {
            _Logger.LogDebug("this is a bug");
            _Logger.LogInformation($"{nameof(Index)}");
            _Logger.LogWarning("this is a waring");
            _Logger.LogError("this is a Error");
            _Logger.LogTrace("this is a Trace");
            _Logger.LogCritical("this is a Critical");

            return new JsonResult(new { Succes = true });
        }
    }
}
