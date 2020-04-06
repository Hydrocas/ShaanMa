///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 23/03/2020 00:41
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.Isartdigital.Common.Audio.Transition {
	[RequireComponent(typeof(AudioSource))]
	public class AudioClick : MonoBehaviour {

		private AudioSource audioSource;

		private void Start () 
		{
			audioSource = GetComponent<AudioSource>();
		}
		
		private void Update () 
		{
			if (Input.GetMouseButtonUp(0))
			{
				audioSource.Play();
			}
		}
	}
}