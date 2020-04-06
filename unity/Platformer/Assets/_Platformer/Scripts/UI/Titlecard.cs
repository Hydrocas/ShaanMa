///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 16/03/2020 11:37
///-----------------------------------------------------------------

using Com.IsartDigital.Common.Objects;
using System;
using UnityEngine;

namespace Com.Isartdigital.Platformer.UI {
	public class Titlecard : ScreenObject {

		public ScreenObjectEventHandler OnContinue;
		private Action DoAction;

		public override void EndAppear()
		{
			base.EndAppear();
			DoAction = DoActionWaitClick;
		}

		public override void Disappear()
		{
			base.Disappear();
			DoAction = DoActionVoid;
		}

		protected override void AppearedUpdate()
		{
			DoAction?.Invoke();
		}

		private void DoActionWaitClick()
		{
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
			{
				OnContinue?.Invoke(this);
				DoAction = DoActionVoid;
			}
		}

		private void DoActionVoid()
		{

		}
	}
}