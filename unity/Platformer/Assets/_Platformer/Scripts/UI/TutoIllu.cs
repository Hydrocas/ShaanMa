///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 21/03/2020 14:28
///-----------------------------------------------------------------

using Com.IsartDigital.Common.Objects;
using UnityEngine;

namespace Com.Isartdigital.Platformer.UI {
	public class TutoIllu : PopupObject {

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