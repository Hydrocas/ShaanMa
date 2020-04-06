///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 05/02/2020 11:55
///-----------------------------------------------------------------

using Com.IsartDigital.Common.Objects;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Isartdigital.Platformer.UI {
	public class OptionScreen : ScreenObject {

		[SerializeField] private Button backButton;

		public override void EndAppear()
		{
			base.EndAppear();
			backButton.onClick.AddListener(Back);
		}

		private void Back()
		{
			screenDisplayer.Remove(this);
		}
	}
}