using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

using KSquared.FantasySportsCoach.Common;

namespace KSquared.FantasySportsCoach.SportsModels
{
	/// <summary>A league of teams.</summary>
	public class League
	{
		#region Constructors

		/// <summary>Creates a new <see cref="League"/>.</summary>
		/// <param name="startingLineupPositions">The available starting lineup positions.</param>
		/// <param name="players">The players in the league.</param>
		/// <param name="teams">The teams in the league.</param>
		/// <param name="games">The games to be played between teams in the league.</param>
		public League(IEnumerable<Position> startingLineupPositions = null, IEnumerable<Player> players = null, IEnumerable<Team> teams = null, IEnumerable<Game> games = null)
		{
			if (startingLineupPositions != null) { this.startingLineupPositions.AddRange(startingLineupPositions); }
			if (players != null) { this.players.AddRange(players); }
			if (teams != null) { this.teams.AddRange(teams); }
			if (games != null) { this.games.AddRange(games); }
		}

		#endregion Constructors
		#region Properties

		private GenericKeyedCollection<string, Team> teams = new GenericKeyedCollection<string, Team>(delegate(Team team)
		{
			if (team == null) { throw new ArgumentNullException("team"); }
			return team.Name;
		});
		/// <summary>List of teams in the league.</summary>
		public KeyedCollection<string, Team> Teams
		{
			get { return this.teams; }
		}

		private List<Game> games = new List<Game>();
		/// <summary>List of games that will be played in the league.</summary>
		public IList<Game> Games
		{
			get { return this.games; }
		}

		private List<Player> players = new List<Player>();
		/// <summary>List of players in the league.</summary>
		public IList<Player> Players
		{
			get { return this.players; }
		}

		private List<Position> startingLineupPositions = new List<Position>(new [] { Position.C, Position.C, Position.LW, Position.LW, Position.RW, Position.RW, Position.D, Position.D, Position.D, Position.D, Position.G, Position.G, });
		/// <summary>The starting positions for each day.</summary>
		public ReadOnlyCollection<Position> StartingLineupPositions
		{
			get { return startingLineupPositions.AsReadOnly(); }
		}

		#endregion Properties
		#region Methods

		#region Utilization

		/// <summary>Computes the efficiency of a roster as a percentage of the possible total starting lineup spots that are filled over a period of time.</summary>
		/// <param name="startDate">The date computation should begin.</param>
		/// <param name="endDate">The date computation should end.</param>
		/// <param name="players">The players that are available to fill starting linueup spots.</param>
		/// <returns>Returns the utilization as a percentage of available starting lineup spots that were filled.</returns>
		/// <remarks>Starting lineups will be generated based on the players.</remarks>
		public double ComputeUtilization(DateTime startDate, DateTime endDate, IEnumerable<Player> players)
		{
			KeyedCollection<DateTime, StartingLineup> startingLineups = this.CreateStartingLineups(startDate, endDate, players);
			return ComputeUtilization(startDate, endDate, startingLineups);
		}

		/// <summary>Computes the efficiency of a roster as a percentage of the possible total starting lineup spots that are filled over a period of time.</summary>
		/// <param name="startDate">The date computation should begin.</param>
		/// <param name="endDate">The date computation should end.</param>
		/// <param name="startingLineups">The starting lineups to use.</param>
		/// <returns>Returns the utilization as a percentage of available starting lineup spots that were filled.</returns>
		public double ComputeUtilization(DateTime startDate, DateTime endDate, KeyedCollection<DateTime, StartingLineup> startingLineups)
		{
			int availablePositions = this.startingLineupPositions.Count * this.GetGameDays(startDate, endDate).Count;
			int usedPositions = startingLineups.Sum(startingLineup => startingLineup.Assignments.Count);
			return (double)usedPositions / (double)availablePositions;
		}

		#endregion Utilization
		#region StartingLineup

		/// <summary>Creates a <see cref="StartingLineup"/> for each day in the date range.</summary>
		/// <param name="startDate">The first day that starting lineups should be created for.</param>
		/// <param name="endDate">The last day that starting lineups should be created for.</param>
		/// <param name="players">The players available to fill starting lineup spots.</param>
		/// <returns>Returns a <see cref="StartingLineup"/> for each day in the date range.</returns>
		public KeyedCollection<DateTime, StartingLineup> CreateStartingLineups(DateTime startDate, DateTime endDate, IEnumerable<Player> players)
		{
			GenericKeyedCollection<DateTime, StartingLineup> startinglineups = new GenericKeyedCollection<DateTime, StartingLineup>(startingLineup => startingLineup.Date.Date);

			if (startDate > endDate) { throw new ArgumentException("End date cannot be before start date.", "endDate"); }
			for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
			{
				startinglineups.Add(this.CreateStartingLineup(date, players));
			}

			return startinglineups;
		}

		/// <summary>Creates a <see cref="StartingLineup"/> for the specified date.</summary>
		/// <param name="date">The date that a starting lineup should be created for.</param>
		/// <param name="players">The players available to fill starting lineup spots.</param>
		/// <returns>Returns a <see cref="StartingLineup"/> for the specified date.</returns>
		public StartingLineup CreateStartingLineup(DateTime date, IEnumerable<Player> players)
		{
			IList<Team> teamsPlaying = this.GetTeamsPlaying(date.Date);
			StartingLineup startingLineup = new StartingLineup(this, date, this.StartingLineupPositions);

			// get a list of which players can play today
			List<Player> playersPlayingToday = new List<Player>(players).FindAll(delegate(Player player)
			{
				return (teamsPlaying.Contains(player.Team) && player.Positions.Count > 0);
			});

			// assign any players that can only be assigned to a single position
			foreach (Player player in playersPlayingToday.ToArray())
			{
				if (player.Positions.Count > 1) { continue; }

				Position position = player.Positions[0];
				if (startingLineup.GetAvailablePositions(position).Count > 0)
				{
					startingLineup.Assignments.Add(new StartingLineupAssignment(player, position));
				}
				// whether or not the player was assigned to a starting lineup spot, he can't be used again
				playersPlayingToday.Remove(player);
			}

			List<Position> availablePositions = new List<Position>(startingLineup.GetAvailablePositions());
			while (availablePositions.Count > 0)
			{
				#region Sort

				// fill positions that have the fewest players that can sastisfy them first
				availablePositions.Sort(delegate(Position left, Position right)
				{
					int leftScore = 0;
					{
						int availablePositionSpots = startingLineup.GetAvailablePositions(left).Count;
						int playersToFillPositions = playersPlayingToday.FindAll(delegate(Player player)
						{
							return player.Positions.Contains(left);
						}).Count;
						leftScore = availablePositionSpots - playersToFillPositions;
					}

					int rightScore = 0;
					{
						int availablePositionSpots = startingLineup.GetAvailablePositions(right).Count;
						int playersToFillPositions = playersPlayingToday.FindAll(delegate(Player player)
						{
							return player.Positions.Contains(right);
						}).Count;
						rightScore = availablePositionSpots - playersToFillPositions;
					}

					return rightScore.CompareTo(leftScore);
				});

				#endregion Sort

				Position position = availablePositions[0];
				List<Player> playersPlayingPosition = playersPlayingToday.FindAll(player => player.Positions.Contains(position));

				// if there are no players left who can fill a position, remove that position from the list of positions left to be filled
				if (playersPlayingPosition.Count <= 0)
				{
					availablePositions.Remove(position);
					continue;
				}

				// find the player who can fill the least number of other positions that remain to be filled
				playersPlayingPosition.Sort(delegate(Player left, Player right)
				{
					int leftScore = 0;
					int rightScore = 0;
					foreach (Position pos in left.Positions) { leftScore += startingLineup.GetAvailablePositions(pos).Count; }
					foreach (Position pos in right.Positions) { rightScore += startingLineup.GetAvailablePositions(pos).Count; }
					return leftScore.CompareTo(rightScore);
				});

				// assign the player that can satisfy the fewest remaining positions that still need to be filled
				startingLineup.Assignments.Add(new StartingLineupAssignment(playersPlayingPosition[0], position));
				playersPlayingToday.Remove(playersPlayingPosition[0]);
				availablePositions.Remove(position);
			}
			return startingLineup;
		}

		#endregion Starting Lineup

		/// <summary>Gets all the games that occur within the date range.</summary>
		/// <param name="startDate">The start of the range.</param>
		/// <param name="endDate">The end of the range.</param>
		/// <returns>Returns all the games that occur within the date range.</returns>
		public IList<Game> GetGames(DateTime startDate, DateTime endDate)
		{
			return new List<Game>(this.Games
				.Where(game => (game.DateTime >= startDate && game.DateTime <= endDate)));
		}

		/// <summary>Gets all the teams playing a game on the specified date.</summary>
		/// <param name="date">The date.</param>
		/// <returns>Returns all the teams playing a game on the specified date.</returns>
		public IList<Team> GetTeamsPlaying(DateTime date)
		{
			return new List<Team>(this.Games
				.Where(game => (game.DateTime.Date == date.Date))
				.SelectMany(game => new Team[] { game.HomeTeam, game.VisitingTeam }));
		}

		/// <summary>Gets all the dates within the specified date range on which at least one game is played.</summary>
		/// <param name="startDate">The start of the range.</param>
		/// <param name="endDate">The end of the range.</param>
		/// <returns>Returns all the dates within the specified date range on which at least one game is played.</returns>
		public IList<DateTime> GetGameDays(DateTime startDate, DateTime endDate)
		{
			return new List<DateTime>(this.Games
				.Where(game => (game.DateTime.Date >= startDate.Date && game.DateTime.Date <= endDate.Date))
				.Select(game => game.DateTime.Date));
		}

		#endregion Methods
	}
}
