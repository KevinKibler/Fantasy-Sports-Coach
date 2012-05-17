using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

using KSquared.FantasySportsCoach.Common;

namespace KSquared.FantasySportsCoach.SportsModels
{
	/// <summary>A starting lineup of players.</summary>
	public class StartingLineup
	{
		#region Constructors

		/// <summary>Creates a new <see cref="StartingLineup"/>.</summary>
		/// <param name="league">The league the starting lineup is for.</param>
		/// <param name="date">The date of the lineup.</param>
		/// <param name="positions">The positions available in the lineup.</param>
		/// <param name="assignments">Assignments of players to positions.</param>
		public StartingLineup(League league, DateTime date, IEnumerable<Position> positions, IEnumerable<StartingLineupAssignment> assignments = null)
		{
			this.League = league;
			this.Date = date;
			this.startingPositions.AddRange(positions);
			this.assignments.CollectionChanging += new EventHandler<NotifyCollectionChangingEventArgs>(assignments_CollectionChanging);
			if (assignments != null) { this.assignments.AddRange(assignments); }
		}

		#endregion Constructors
		#region Properties

		/// <summary>The league the lineup is for.</summary>
		public League League { get; set; }

		/// <summary>The date of the lineup.</summary>
		public DateTime Date { get; set; }

		private List<Position> startingPositions = new List<Position>();
		/// <summary>The positions available for the lineup.</summary>
		public ReadOnlyCollection<Position> StartingPositions
		{
			get { return this.startingPositions.AsReadOnly(); }
		}

		private ObservableCollectionEx<StartingLineupAssignment> assignments = new ObservableCollectionEx<StartingLineupAssignment>();
		/// <summary>Assignments of players to positions.</summary>
		public IList<StartingLineupAssignment> Assignments
		{
			get { return assignments; }
		}

		#endregion Properties
		#region Events

		private void assignments_CollectionChanging(object sender, NotifyCollectionChangingEventArgs e)
		{
			// TODO: implement collection changing restrictions
			switch (e.Action)
			{
				case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
					foreach (StartingLineupAssignment newAssignment in e.NewItems)
					{
						if (!newAssignment.Player.Positions.Contains(newAssignment.Position)) { throw new ArgumentException("Player cannot fill this position."); }
						if (this.Assignments.Where(assignment => assignment.Player == newAssignment.Player).Count() > 0) { throw new ArgumentException("Player cannot be assigned to multiple positions."); }
						if (this.GetAvailablePositions(newAssignment.Position).Count <= 0) { throw new ArgumentException("No available positions of this type are avilable."); }
						if (!newAssignment.Player.IsPlaying(this.Date)) { throw new ArgumentException("Player is not playing on the specified date."); }
					}
					break;
				case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
					//TODO: throw an exception if player slot is already filled
					break;
				case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
				case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
				case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
				default:
					break;
			}
		}

		#endregion Events
		#region Methods

		/// <summary>Gets the remaining available positions.</summary>
		/// <returns>Returns the remaining available positions.</returns>
		public IList<Position> GetAvailablePositions()
		{
			return this.GetAvailablePositions((Position[])Enum.GetValues(typeof(Position)));
		}

		/// <summary>Gets the remaining available positions for the specified position.</summary>
		/// <param name="position">The position to filter by.</param>
		/// <returns>Returns the remaining available positions.</returns>
		public IList<Position> GetAvailablePositions(Position position)
		{
			return this.GetAvailablePositions(new[] { position });
		}

		/// <summary>Gets the remaining available positions for the specified position.</summary>
		/// <param name="positions">The positions to filter by.</param>
		/// <returns>Returns the remaining available positions.</returns>
		public IList<Position> GetAvailablePositions(IEnumerable<Position> positions)
		{
			List<Position> availablePositions = new List<Position>();
			List<Position> positionList = new List<Position>(positions);
			foreach (var item in this.startingPositions)
			{
				if (positionList.Contains(item)) { availablePositions.Add(item); }
			}
			foreach (StartingLineupAssignment assignment in this.Assignments)
			{
				availablePositions.Remove(assignment.Position);
			}
			return availablePositions;
		}

		/// <summary>Gets the total number of positions in the lineup.</summary>
		/// <param name="position">The position to filter by.</param>
		/// <returns>Returns a count of the number of positions.</returns>
		public int GetTotalPositionCount(Position position)
		{
			return new List<Position>(this.StartingPositions).FindAll(delegate(Position pos) { return (pos == position); }).Count;
		}

		#endregion Methods
	}

	/// <summary>An assignment of a player to a position in a starting lineup.</summary>
	[DebuggerDisplay("{ToString()}")]
	public class StartingLineupAssignment
	{
		#region Constructors

		/// <summary>Creates a new <see cref="StartingLineupAssignment"/>.</summary>
		/// <param name="player">The player.</param>
		/// <param name="position">The position.</param>
		public StartingLineupAssignment(Player player, Position position)
		{
			if (player == null) { throw new ArgumentNullException("player"); }
			if (!player.Positions.Contains(position)) { throw new ArgumentException("Player cannot play specified position.", "player"); }

			this.player = player;
			this.position = position;
		}

		#endregion Constructors
		#region Properties

		private Player player = null;
		/// <summary>The player.</summary>
		public Player Player
		{
			get { return this.player; }
		}

		private Position position;
		/// <summary>The position</summary>
		public Position Position
		{
			get { return this.position; }
		}

		#endregion Properties
		#region Methods

		/// <summary>Gets the string representation.</summary>
		/// <returns>Returns the string representation.</returns>
		public override string ToString()
		{
			return string.Format("{0}: {1}", this.Position, this.Player);
		}

		#endregion Methods
	}
}
