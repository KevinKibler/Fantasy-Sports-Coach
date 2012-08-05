using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FantasySportsCoach.Models;

namespace FantasySportsCoach.Controllers
{
	public class HomeController : Controller
	{
		public ViewResult Index()
		{
			ViewBag.Message = "Welcome to ASP.NET MVC!";

			return View();
		}

		public ViewResult About()
		{
			ViewBag.ProjectUrl = @"https://github.com/CodeSavvyGeek/Fantasy-Sports-Coach";
			return View();
		}
	}
}
