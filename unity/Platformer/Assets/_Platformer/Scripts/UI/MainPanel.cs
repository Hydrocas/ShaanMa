///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 21/01/2020 13:49
///-----------------------------------------------------------------

using Com.IsartDigital.Common.Objects;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Isartdigital.Platformer.UI {
	public class MainPanel : ScreenObject {

		[SerializeField] private LevelButton playButton;
		[SerializeField] private Button levelSelectionButton;
		[SerializeField] private Button optionButton;
		[SerializeField] private Button creditsButton;

		public LevelButton PlayButton => playButton;
		public Button LevelSelectionButton => levelSelectionButton;
		public Button OptionButton => optionButton;
		public Button CreditsButton => creditsButton;

		public override void EndAppear()
		{
			base.EndAppear();
			playButton.Init();
		}
	}
}