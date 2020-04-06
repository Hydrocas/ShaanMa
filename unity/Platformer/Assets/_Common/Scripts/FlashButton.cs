///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 16/03/2020 21:37
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Com.Isartdigital.Common {
	[RequireComponent(typeof(Button))]
	public class FlashButton : MonoBehaviour {

		[SerializeField] private GameObject gfxIdle;
		[SerializeField] private GameObject gfxHover;

		private void Awake()
		{
			Button button = GetComponent<Button>();

			//button.OnPointerUp += lol;
		}

		private void lol(PointerEventData obj)
		{
			throw new NotImplementedException();
		}
	}
}