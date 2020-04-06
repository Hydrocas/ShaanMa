///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 05/02/2020 15:54
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Isartdigital.Platformer.UI {

	public delegate void LevelButtonEventHandler(LevelButton sender);

	[RequireComponent(typeof(Button))]
	public class LevelButton : MonoBehaviour {

		[SerializeField] private int levelIndex;
		public int LevelIndex => levelIndex;

		public event LevelButtonEventHandler OnClick;

		public void Init () 
		{
			GetComponent<Button>().onClick.AddListener(Button_OnClick);
		}

		private void Button_OnClick()
		{
			OnClick?.Invoke(this);
		}
	}
}