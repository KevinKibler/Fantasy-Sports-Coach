using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSquared.FantasySportsCoach.SportsModels
{
	/// <summary>Positions a player can play.</summary>
	public enum Position
	{
		/// <summary>No position</summary>
		None = 0x0,
		/// <summary>Center</summary>
		C = 0x1,
		/// <summary>Left Wing</summary>
		LW = 0x2,
		/// <summary>Right Wing</summary>
		RW = 0x4,
		/// <summary>Defense</summary>
		D = 0x8,
		/// <summary>Goaltender</summary>
		G = 0x10,
	}

	//public class Position1
	//{
	//    #region Constructors

	//    public Position1(string shortName, string fullName)
	//    {
	//        this.ShortName = shortName;
	//        this.FullName = fullName;
	//    }

	//    #endregion

	//    #region Properties

	//    /// <summary>Gets or sets the short name of a position.</summary>
	//    public string ShortName { get; set; }

	//    /// <summary>Gets or sets the full name of a position</summary>
	//    public string FullName { get; set; }

	//    #endregion
	//}
}
