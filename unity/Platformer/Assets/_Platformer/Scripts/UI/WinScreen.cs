///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 21/01/2020 13:52
///-----------------------------------------------------------------

using Com.IsartDigital.Common.Objects;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Isartdigital.Platformer.UI {
	public class WinScreen : ScreenObject {

		[SerializeField] private Button retryButton;
		[SerializeField] private Button nextButton;

		public Button RetryButton => retryButton;
		public Button NextButton => nextButton;

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