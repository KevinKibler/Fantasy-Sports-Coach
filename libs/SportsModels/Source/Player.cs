using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace KSquared.FantasySportsCoach.SportsModels
{
	/// <summary>A player.</summary>
	[DebuggerDisplay("{ToString()}")]
	public class Player
	{
		#region Constructors

		/// <summary>Creates a new <see cref="Player"/>.</summary>
		/// <param name="name">The name of the player.</param>
		/// <param name="team">The team the player plays for.</param>
		/// <param name="positions">The positions the player can play.</param>
		public Player(string name, Team team = null, IEnumerable<Position> positions = null)
		{
			this.Name = name;
			this.Team = team;
			if (positions != null) { this.positions.AddRange(positions); }
		}

		#endregion Constructors
		#region Properties

		/// <summary>The name of the player</summary>
		public string Name { get; set; }

		/// <summary>Gets or sets the team the player is a member of.</summary>
		public Team Team { get; set; }

		private List<Position> positions = new List<Position>();
		/// <summary>Gets the positions the player can play.</summary>
		public IList<Position> Positions
		{
			get { return this.positions; }
		}

		#endregion Properties
		#region Methods

		/// <summary>Gets a value indicating whether or not this player is playing on the specified date.</summary>
		/// <param name="date">The date to check.</param>
		/// <returns>Returns true if the player's team is playing on the specified date, false otherwise.</returns>
		public bool IsPlaying(DateTime date)
		{
			if (this.Team == null) { return false; }
			return this.Team.IsPlaying(date);
		}

		/// <summary>Gets the string representation.</summary>
		/// <returns>Returns the string representation</returns>
		public override string ToString()
		{
			string positionString = string.Empty;
			foreach (Position position in this.Positions)
			{
				positionString += string.Format("{0}, ", position.ToString());
			}
			positionString = positionString.TrimEnd(',', ' ');

			return string.Format("{0}; {1}; {2}", this.Name, this.Team, positionString);
		}

		#endregion Methods
	}
}
