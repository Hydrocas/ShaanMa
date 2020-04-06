///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 01/12/2019 20:52
///-----------------------------------------------------------------

using Com.Isartdigital.Common.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Com.DefaultCompany.ParticleTest.Common {
    [RequireComponent(typeof(Button))]
	public class ButtonSound : MonoBehaviour {

        private SoundTag soundName = SoundTag.Button;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(Button_OnClick);
        }

        private void Button_OnClick()
        {
            AudioManager.Instance.Play(soundName);
        }

        private void OnDestroy()
        {
            GetComponent<Button>().onClick.RemoveListener(Button_OnClick);
        }
    }
}