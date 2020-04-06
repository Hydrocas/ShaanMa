///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 21/01/2020 13:58
///-----------------------------------------------------------------

using Com.IsartDigital.Common.Objects;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Isartdigital.Platformer.UI {
	public class LoginScreen : ScreenObject {

		[SerializeField] private InputField inputField;
		public InputField InputField => inputField;
	}
}