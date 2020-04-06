///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 21/01/2020 14:00
///-----------------------------------------------------------------

using Com.Isartdigital.Platformer.Managers;
using Com.IsartDigital.Common.Objects;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Isartdigital.Platformer.UI {
	public class PauseScreen : ScreenObject {

		[SerializeField] private OptionScreen optionScreen;
		[SerializeField] private Button resumeButton;
		[SerializeField] private Button optionBtn;
		[SerializeField] private Button quitButton;

		[SerializeField] private Text timeTxt;
		[SerializeField] private Text deathTxt;
		[SerializeField] private LevelManager levelManager;

		public Button QuitButton => quitButton;

		public override void Appear()
		{
			base.Appear();
			deathTxt.text = levelManager.DeathCount.ToString();
			timeTxt.text = ((int)levelManager.TimeSpan).ToString() + "s";
		}

		public override void EndAppear()
		{
			base.EndAppear();
			Time.timeScale = 0;
			optionBtn.onClick.AddListener(DisplayOption);
			resumeButton.onClick.AddListener(Resume);
		}

		private void DisplayOption()
		{
			screenDisplayer.Display(optionScreen);
		}

		private void Resume()
		{
			screenDisplayer.Remove(this);
		}

		public override void EndDisappear()
		{
			base.EndDisappear();
			Time.timeScale = 1;
		}
	}
}