using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FantasySportsCoach.Models;
using KSquared.FantasySportsCoach.Website.Models;

namespace FantasySportsCoach.Controllers
{
	public class HomeController : Controller
	{
		#region Fields

		private FantasySportsCoachDb database = new FantasySportsCoachDb();

		#endregion

		public ViewResult Index()
		{
			ViewBag.Message = string.Format("Number of leagues: {0}", database.Leagues.Count());

			return View();
		}

		public ViewResult About()
		{
			ViewBag.ProjectUrl = @"https://github.com/CodeSavvyGeek/Fantasy-Sports-Coach";
			return View();
		}
	}
}
