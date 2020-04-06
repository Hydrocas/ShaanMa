using UnityEngine;
using UnityEngine.UI;

namespace Com.DefaultCompany.ParticleTest.Common {
    [RequireComponent(typeof(Toggle)), ExecuteInEditMode]
    public class ToggleSwitchSprite : MonoBehaviour {

        [SerializeField] private Sprite spriteIsOff = null;
        private Sprite spriteIsOn;
        private Image image;

        private void Start () {
            Toggle toggle = GetComponent<Toggle>();
            image         = toggle.image;
            spriteIsOn    = image.sprite;
            toggle.onValueChanged.AddListener(Toggle_OnValueChanged);
        }

        private void Toggle_OnValueChanged(bool isOn)
        {
            image.sprite = isOn ? spriteIsOn : spriteIsOff;
        }
    }
}