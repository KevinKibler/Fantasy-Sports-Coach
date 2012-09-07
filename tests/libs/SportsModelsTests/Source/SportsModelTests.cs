using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSquared.FantasySportsCoach.Common;
using KSquared.FantasySportsCoach.SportsModels;

using NUnit.Framework;
using System.IO;

namespace KSquared.FantasySportsCoach.SportsModels.Tests
{
	/// <summary>Tests for Sports Models.</summary>
	[TestFixture]
	public static class SportsModelTests
	{
		#region Fields

		private static League league = null;

		#endregion Fields
		#region Methods

		#region Setup

		/// <summary>Setup before each tests.</summary>
		[SetUp]
		public static void Setup()
		{
			//MemoryStream stream = new MemoryStream(Properties.Resources.NHLSchedule2010);
			//League league = LeagueCsvAdaptor.LoadCsv(stream);
			SportsModelTests.league = new League();
		}

		#endregion Setup
		#region Tests

		/// <summary>Utilization test with no players on the roster.</summary>
		[Test]
		public static void TestUtilizationNoPlayers()
		{
			Team t1 = new Team("T1", league);
			league.Teams.Add(t1);

			Team t2 = new Team("T2", league);
			league.Teams.Add(t2);

			league.Games.Add(new Game(t1, t2, DateTime.Today));

			double util = league.ComputeUtilization(DateTime.Today, DateTime.Today, new Player[] { });
			Assert.AreEqual(0.0, util);
		}

		/// <summary>Utilization test with all players playing exactly 1 position.</summary>
		[Test]
		public static void TestUtilizationPlayerPosition1()
		{
			Team t1 = new Team("T1", league);
			league.Teams.Add(t1);

			Team t2 = new Team("T2", league);
			league.Teams.Add(t2);

			league.Games.Add(new Game(t1, t2, DateTime.Today));

			var players = new List<Player>();
			foreach (var position in league.StartingLineupPositions)
			{
				Player p = new Player(string.Empty, t1, new[] { position });
				players.Add(p);
				t1.Players.Add(p);
			}
			double util = league.ComputeUtilization(DateTime.Today, DateTime.Today, players);
			Assert.AreEqual(1.0, util);
		}

		/// <summary>Utilization test with all players playing all positions.</summary>
		[Test]
		public static void TestUtilizationPlayerPositionsAll()
		{
			Team t1 = new Team("T1", league);
			league.Teams.Add(t1);

			Team t2 = new Team("T2", league);
			league.Teams.Add(t2);

			league.Games.Add(new Game(t1, t2, DateTime.Today));

			var positions = new List<Position>();
			foreach (Position item in Enum.GetValues(typeof(Position))) { positions.Add(item); }

			var players = new List<Player>();
			foreach (var position in league.StartingLineupPositions)
			{
				Player p = new Player(string.Empty, t1, positions);
				players.Add(p);
				t1.Players.Add(p);
			}
			double util = league.ComputeUtilization(DateTime.Today, DateTime.Today, players);
			Assert.AreEqual(1.0, util);
		}

		/// <summary>Utilization test with players playing a mix of 1 and multiple positions.</summary>
		[Test]
		public static void TestUtilizationPlayerPositionsMixed1()
		{
			Team t1 = new Team("T1", league);
			league.Teams.Add(t1);

			Team t2 = new Team("T2", league);
			league.Teams.Add(t2);

			league.Games.Add(new Game(t1, t2, DateTime.Today));

			var players = new List<Player>();
			players.Add(new Player(string.Empty, t1, new Position[] { Position.C }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.C, Position.LW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.C, Position.RW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.LW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.RW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.LW, Position.RW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.D }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.D }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.D }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.D }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.G }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.G }));
			foreach (var player in players) { t1.Players.Add(player); }

			double util = league.ComputeUtilization(DateTime.Today, DateTime.Today, players);
			Assert.AreEqual(1.0, util);
		}

		/// <summary>Utilization test with players playing a mix of 1 and multiple positions.</summary>
		[Test]
		public static void TestUtilizationPlayerPositionsMixed2()
		{
			Team t1 = new Team("T1", league);
			league.Teams.Add(t1);

			Team t2 = new Team("T2", league);
			league.Teams.Add(t2);

			league.Games.Add(new Game(t1, t2, DateTime.Today));

			var players = new List<Player>();
			players.Add(new Player(string.Empty, t1, new Position[] { Position.C }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.C, Position.LW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.C, Position.RW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.LW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.C, Position.RW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.RW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.RW, Position.D }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.D }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.D }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.D }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.G }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.G }));
			foreach (var player in players) { t1.Players.Add(player); }

			double util = league.ComputeUtilization(DateTime.Today, DateTime.Today, players);
			Assert.AreEqual(1.0, util);
		}

		/// <summary>Utilization test with players playing a mix of 1 and multiple positions.</summary>
		[Test]
		public static void TestUtilizationPlayerPositionsMixed3()
		{
			Team t1 = new Team("T1", league);
			league.Teams.Add(t1);

			Team t2 = new Team("T2", league);
			league.Teams.Add(t2);

			league.Games.Add(new Game(t1, t2, DateTime.Today));

			var players = new List<Player>();
			players.Add(new Player(string.Empty,t1, new Position[] { Position.C, Position.LW, Position.G }));
			players.Add(new Player(string.Empty,t1, new Position[] { Position.C, Position.LW, Position.G }));
			players.Add(new Player(string.Empty,t1, new Position[] { Position.C, Position.RW }));
			players.Add(new Player(string.Empty,t1, new Position[] { Position.LW }));
			players.Add(new Player(string.Empty,t1, new Position[] { Position.C, Position.RW }));
			players.Add(new Player(string.Empty,t1, new Position[] { Position.C, Position.LW }));
			players.Add(new Player(string.Empty,t1, new Position[] { Position.RW, Position.D }));
			players.Add(new Player(string.Empty,t1, new Position[] { Position.RW, Position.D }));
			players.Add(new Player(string.Empty,t1, new Position[] { Position.RW, Position.D }));
			players.Add(new Player(string.Empty,t1, new Position[] { Position.RW, Position.D }));
			players.Add(new Player(string.Empty,t1, new Position[] { Position.G }));
			players.Add(new Player(string.Empty,t1, new Position[] { Position.G }));
			foreach (var player in players) { t1.Players.Add(player); }

			double util = league.ComputeUtilization(DateTime.Today, DateTime.Today, players);
			Assert.AreEqual(1.0, util);
		}

		/// <summary>Utilization test with players playing a mix of 1 and multiple positions.</summary>
		[Test]
		public static void TestUtilizationPlayerPositionsMixed4()
		{
			Team t1 = new Team("T1", league);
			league.Teams.Add(t1);

			Team t2 = new Team("T2", league);
			league.Teams.Add(t2);

			league.Games.Add(new Game(t1, t2, DateTime.Today));

			var players = new List<Player>();
			players.Add(new Player(string.Empty, t1, new Position[] { Position.C, Position.LW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.C, Position.LW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.C, Position.RW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.C, Position.RW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.C, Position.LW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.LW, Position.RW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.D }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.D }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.RW, Position.D }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.RW, Position.D }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.G }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.G }));
			foreach (var player in players) { t1.Players.Add(player); }

			double util = league.ComputeUtilization(DateTime.Today, DateTime.Today, players);
			Assert.AreEqual(1.0, util);
		}

		/// <summary>Utilization test with players playing a mix of 1 and multiple positions.</summary>
		[Test]
		public static void TestUtilizationPlayerPositionsMixed5()
		{
			Team t1 = new Team("T1", league);
			league.Teams.Add(t1);

			Team t2 = new Team("T2", league);
			league.Teams.Add(t2);

			league.Games.Add(new Game(t1, t2, DateTime.Today));

			var players = new List<Player>();
			players.Add(new Player(string.Empty, t1, new Position[] { Position.C, Position.LW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.C, Position.LW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.C, Position.RW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.C, Position.RW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.C, Position.LW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.LW, Position.RW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.D }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.D }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.RW, Position.D }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.RW, Position.D }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.G }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.G }));
			foreach (var player in players) { t1.Players.Add(player); }

			double util = league.ComputeUtilization(DateTime.Today, DateTime.Today, players);
			Assert.AreEqual(1.0, util);
		}

		/// <summary>Utilization test with players playing a mix of 1 and multiple positions.</summary>
		[Test]
		public static void TestUtilizationPlayerPositionsMixed6()
		{
			Team t1 = new Team("T1", league);
			league.Teams.Add(t1);

			Team t2 = new Team("T2", league);
			league.Teams.Add(t2);

			league.Games.Add(new Game(t1, t2, DateTime.Today));

			var players = new List<Player>();
			players.Add(new Player(string.Empty, t1, new Position[] { Position.RW, Position.D }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.RW, Position.D }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.C, Position.RW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.C, Position.RW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.C, Position.LW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.RW, Position.D }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.C, Position.LW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.LW, Position.RW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.D, Position.LW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.D, Position.LW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.G, Position.LW }));
			players.Add(new Player(string.Empty, t1, new Position[] { Position.G, Position.RW }));
			foreach (var player in players) { t1.Players.Add(player); }

			double util = league.ComputeUtilization(DateTime.Today, DateTime.Today, players);
			Assert.AreEqual(1.0, util);
		}

		#endregion Tests

		#endregion
	}
}
