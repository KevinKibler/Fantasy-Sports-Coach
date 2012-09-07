using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using KSquared.FantasySportsCoach.SportsModels;
using System.IO;

namespace KSquared.FantasySportsCoach.Website.Models
{
	/// <summary>Represents the database for the Fantasy Sports Coach application.</summary>
	public class FantasySportsCoachDb : DbContext
	{
		#region Properties

		/// <summary>Data for all the supported leagues.</summary>
		public DbSet<League> Leagues { get; set; }

		#endregion
	}

	/// <summary>Initializer for <see cref="FantasySportsCoachDb"/>.</summary>
	public class FantasySportsCoachDbInitializer : DropCreateDatabaseIfModelChanges<FantasySportsCoachDb>
	{
		/// <summary>Seeds the specified context with test data.</summary>
		/// <param name="context">The context to seed.</param>
		protected override void Seed(FantasySportsCoachDb context)
		{
			base.Seed(context);

			MemoryStream stream = new MemoryStream(Properties.Resources.NHLSchedule2010);
			League league = LeagueCsvAdaptor.LoadCsv(stream);
			context.Leagues.Add(league);

			context.SaveChanges();
		}
	}
}