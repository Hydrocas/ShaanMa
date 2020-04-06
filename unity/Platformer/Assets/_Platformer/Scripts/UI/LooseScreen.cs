///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 21/01/2020 13:51
///-----------------------------------------------------------------

using Com.IsartDigital.Common.Objects;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Isartdigital.Platformer.UI {
	public class LooseScreen : ScreenObject {

		[SerializeField] private Button quitButton;
		[SerializeField] private Button retryButton;

		public Button QuitButton => quitButton;
		public Button RetryButton => retryButton;

		public override void EndAppear()
		{
			base.EndAppear();
			Time.timeScale = 0;
		}

		public override void EndDisappear()
		{
			base.EndDisappear();
			Time.timeScale = 1;
		}
	}
}