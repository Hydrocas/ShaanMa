///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 21/01/2020 13:57
///-----------------------------------------------------------------

using Com.Isartdigital.Platformer.Data;
using Com.IsartDigital.Common.Objects;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Isartdigital.Platformer.UI {
	public class LevelSelection : ScreenObject {

		[SerializeField] private Button backButton;
		[SerializeField] private LevelButton level1Button;
		[SerializeField] private LevelButton level2Button;

		[SerializeField] private Text timeLvl1;
		[SerializeField] private Text scoreLvl1;
		[SerializeField] private Text timeLvl2;
		[SerializeField] private Text scoreLvl2;

		public Button BackButton => backButton;
		public LevelButton Level2Button => level2Button;
		public LevelButton Level1Button => level1Button;

		public override void EndAppear()
		{
			base.EndAppear();

			level1Button.Init();
			level2Button.Init();
		}

		public void SetData(User user)
		{
			if (user.levelSaves[0].timeSpan != 0)
			{
				timeLvl1.text = ((int)user.levelSaves[0].timeSpan).ToString() + "s";
				scoreLvl1.text = user.levelSaves[0].lifeCount.ToString();
			}
			else
			{
				timeLvl1.text = "-";
				scoreLvl1.text = "-";
			}

			if (user.levelSaves[1].timeSpan != 0)
			{
				timeLvl2.text = ((int)user.levelSaves[1].timeSpan).ToString() + "s";
				scoreLvl2.text = user.levelSaves[1].lifeCount.ToString();
			}
			else
			{
				timeLvl2.text = "-";
				scoreLvl2.text = "-";
			}
		}
	}
}