using SecretaryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecretaryService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            MessageRepository messageRepository = new MessageRepository();
            messageRepository.GetAllMessages();
            return View();
        }
    }
}
