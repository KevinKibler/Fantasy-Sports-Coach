using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace KSquared.FantasySportsCoach.SportsModels
{
	/// <summary>Loads data for a <see cref="League"/> from a CSV file.</summary>
	public static class LeagueCsvAdaptor
	{
		#region Methods

		/// <summary>Creates a new league from a data file.</summary>
		/// <param name="dataFilePath">The path to the data file.</param>
		/// <returns>Returns a new league, initialized with the data.</returns>
		public static League LoadCsv(string dataFilePath)
		{
			return LeagueCsvAdaptor.loadCsvData(File.ReadAllText(dataFilePath));
		}

		/// <summary>Creates a new league from a data stream.</summary>
		/// <param name="dataStream">The stream containing the data.</param>
		/// <returns>Returns a new league, initialized with the data.</returns>
		public static League LoadCsv(Stream dataStream)
		{
			string data = string.Empty;
			using (StreamReader streamReader = new StreamReader(dataStream))
			{
				data = streamReader.ReadToEnd();
			}
			return LeagueCsvAdaptor.loadCsvData(data);
		}

		/// <summary>Creates a new league from data.</summary>
		/// <param name="data">The data file to initialize the league with.</param>
		/// <returns>Returns a new league, initialized with the data.</returns>
		private static League loadCsvData(string data)
		{
			League league = new League();
			bool firstLine = true;
			foreach (string line in data.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
			{
				if (firstLine)
				{
					firstLine = false;
					continue;
				}

				string[] parts = line.Split(',');
				DateTime gameDateTime = DateTime.Parse(parts[0] + "," + parts[3]);
				Team visitingTeam = LeagueCsvAdaptor.resolveTeam(league, parts[1].Replace("\"", ""));
				Team homeTeam = LeagueCsvAdaptor.resolveTeam(league, parts[2].Replace("\"", ""));
				Game game = new Game(homeTeam, visitingTeam, gameDateTime);

				league.Games.Add(game);
			}
			return league;
		}

		private static Team resolveTeam(League league, string teamName)
		{
			Team team;
			if (league.Teams.Contains(teamName))
			{
				team = league.Teams[teamName];
			}
			else
			{
				team = new Team(teamName, league);
				league.Teams.Add(team);
			}
			return team;
		}

		#endregion Methods
	}
}
