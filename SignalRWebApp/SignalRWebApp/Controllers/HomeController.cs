using Microsoft.AspNetCore.Mvc;
using SignalRWebApp.Models;
using SignalRWebApp.Service;
using System.Diagnostics;

namespace SignalRWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _user;
        public HomeController(ILogger<HomeController> logger, IUserService user)
        {
            _logger = logger;
            _user = user;
        }

        public bool CodeFirst()
        {
            return _user.CodeFirst();
        }

        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetString("userName");
            if (string.IsNullOrEmpty(userName))
            {
                //说明用户信息不存在，未登录
                return Redirect("/Home/Login");
            }
            ViewBag.UserName = userName;
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult LogOut()
        {
            // 清除Session和Cookie
            HttpContext.Session.Remove("userName");
            Response.Cookies.Delete("userName");
            // 重定向到登录页
            return Redirect("/Home/Login");
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public IActionResult Submit(string name, string password)
        {
            UserInfo userInfo = _user.GetUser(name, password);
            if (string.IsNullOrEmpty(userInfo.Id))
            {
                return Redirect("/Home/Error");
            }
            HttpContext.Session.SetString("userName", userInfo.Name);
            Response.Cookies.Append("userName", userInfo.Name);
            return Redirect("/");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public JsonResult GetMessages(int pageIndex, int pageSize)
        {
            return Json(_user.GetMessages(pageIndex, pageSize));
        }
    }
}