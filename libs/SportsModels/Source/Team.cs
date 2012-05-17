using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace KSquared.FantasySportsCoach.SportsModels
{
	/// <summary>A team of players.</summary>
	[DebuggerDisplay("{Name}")]
	public class Team
	{
		#region Constructors

		/// <summary>Creates a <see cref="Team"/>.</summary>
		/// <param name="league">The league the team is a member of.</param>
		/// <param name="name">The name of the team.</param>
		/// <param name="players">Players on the team.</param>
		public Team(string name, League league = null, IEnumerable<Player> players = null)
		{
			if (string.IsNullOrEmpty(name)) { throw new ArgumentException("name cannot be null or empty", "name"); }

			this.League = league;
			this.Name = name;
			if (players != null) { this.players.AddRange(players); }
		}

		#endregion Constructors
		#region Properties

		/// <summary>Gets or sets the league the team belongs to.</summary>
		public League League { get; set; }

		/// <summary>Gets or sets the name of the team.</summary>
		public string Name { get; set; }

		private List<Player> players = new List<Player>();
		/// <summary>Gets the players on the team.</summary>
		public IList<Player> Players
		{
			get { return this.players; }
		}

		#endregion Properties
		#region Methods

		/// <summary>Gets a value indicating whether or not this team is playing on the specified date.</summary>
		/// <param name="date">The date to check.</param>
		/// <returns>Returns true if the team is playing on the specified date, false otherwise.</returns>
		public bool IsPlaying(DateTime date)
		{
			if (this.League == null) { return false; }
			return this.League.GetTeamsPlaying(date).Contains(this);
		}

		#endregion Methods
	}
}
