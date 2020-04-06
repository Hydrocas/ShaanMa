///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 27/01/2020 16:17
///-----------------------------------------------------------------

using UnityEngine;
using System;

namespace Com.Isartdigital.Platformer.Data {
	[Serializable]
	public class LevelSave {

		public float timeSpan;
		public int score;
		public int lifeCount;

		public LevelSave(float timeSpan, int score, int lifeCount)
		{
			this.timeSpan = timeSpan;
			this.score = score;
			this.lifeCount = lifeCount;
		}

		public LevelSave()
		{

		}

		public LevelSave Combine(LevelSave levelSave)
		{
			if (levelSave.timeSpan == 0)
			{
				return new LevelSave(this.timeSpan, this.score, this.lifeCount);
			}

			float timeSpan = Math.Min(this.timeSpan, levelSave.timeSpan);
			int score = Math.Max(this.score, levelSave.score);
			int lifeCount = Math.Max(this.lifeCount, levelSave.lifeCount);

			return new LevelSave(timeSpan, score, lifeCount);
		}
	}
}