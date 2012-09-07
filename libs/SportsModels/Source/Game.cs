using System;
using System.Collections.Generic;
using System.Text;

namespace KSquared.FantasySportsCoach.SportsModels
{
	/// <summary>A game played between two teams.</summary>
	public class Game
	{
		#region Constructors

		/// <summary>Creates a <see cref="Game"/>.</summary>
		/// <param name="homeTeam">The home team.</param>
		/// <param name="visitingTeam">The visiting team.</param>
		/// <param name="dateTime">The date of the game.</param>
		public Game(Team homeTeam, Team visitingTeam, DateTime dateTime)
		{
			this.HomeTeam = homeTeam;
			this.VisitingTeam = visitingTeam;
			this.DateTime = dateTime;
		}

		#endregion Constructors
		#region Properties

		/// <summary>Unique Id.</summary>
		public int Id { get; set; }

		/// <summary>The home team.</summary>
		public Team HomeTeam { get; set; }

		/// <summary>The visiting team.</summary>
		public Team VisitingTeam { get; set; }

		/// <summary>The date and time of the game.</summary>
		public DateTime DateTime { get; set; }

		#endregion Properties
	}
}
