///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 21/01/2020 13:50
///-----------------------------------------------------------------

using System;
using Com.IsartDigital.Common.Objects;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Isartdigital.Platformer.UI {
	public class Hud : ScreenObject {

		[SerializeField] private Button pauseButton;
		[SerializeField] private Button winButton;
		[SerializeField] private Button looseButton;
        [SerializeField] private Image timeBar;


		public Button PauseButton => pauseButton;
		public Button LooseButton => looseButton;
		public Button WinButton => winButton;

        private float baseTimeBarScaleX;

        private void Start() {
        }

        internal void OnTimeChanged(float timePercent) 
        {
            timeBar.fillAmount = 1 - timePercent;
        }
    }
}