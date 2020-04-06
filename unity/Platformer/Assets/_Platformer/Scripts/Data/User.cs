///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 27/01/2020 16:11
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Isartdigital.Platformer.Data {
	[Serializable]
	public class User {

		public string username;
		public LevelSave[] levelSaves;
		private const int MAX_LEVEL = 2;

		public User(string username, LevelSave[] levelSaves = null)
		{
			this.username = username;
			this.levelSaves = levelSaves ?? new LevelSave[MAX_LEVEL];
		}
	}
}