using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FantasySportsCoach.Controllers;

namespace FantasySportsCoachTests
{
	[TestFixture]
	public class HomeControllerAboutTests
	{
		[Test]
		public void ViewBagProjectUrl()
		{
			var controller = new HomeController();
			var result = controller.About();
			Assert.IsNotNull(result.ViewBag.ProjectUrl);
		}
	}
}
