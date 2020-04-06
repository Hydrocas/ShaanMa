///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 21/01/2020 13:55
///-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Com.IsartDigital.Common.Objects
{
    public class PopupObject : ScreenObject
    {
		[SerializeField] private Button closeButton;
		public override void EndAppear()
		{
			base.EndAppear();
			closeButton.onClick.AddListener(Close);
		}

		private void Close()
		{
			screenDisplayer.Remove(this);
		}
	}
}
