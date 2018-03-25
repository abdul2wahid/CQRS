using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CQRS.Commands;
using CQRS.Configurations;
using Microsoft.AspNetCore.Mvc;
using NOSQL_Read_Project.Configurations;

namespace AccountManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var infos = Configuration_Read.Instance().ReadModel.UserInfoList;
            return View(infos);
        }

        public ActionResult Admin()
        {
            var infos = Configuration_Read.Instance().ReadModel.GetUsers();;
            return View(infos);
        }

     

      

        [HttpGet]
        public ActionResult NewUser()
        {
            var command = new CreateUserCommand { UserId = Guid.NewGuid()};
            return View(command);
        }

        [HttpGet]
        public ActionResult Users(Guid notificationID)
        {
            var notifications = Configuration_Read.Instance().ReadModel.UserInfoList.Where(x => x.Id == notificationID).ToList();
            return PartialView(notifications);
        }

        [HttpPost]
        public ActionResult NewUser(CreateUserCommand command)
        {
            Configuration.Instance().Bus.Handle(command);
            return View("Registered");
        }
    }
}
