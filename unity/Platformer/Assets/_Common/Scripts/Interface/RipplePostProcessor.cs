///-----------------------------------------------------------------
/// Author : Loïc Jacob
/// Date : 10/02/2020 17:44
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.Isartdigital.Common.Interface {
	public class RipplePostProcessor : MonoBehaviour {

        [SerializeField] private Material rippleMaterial;

        private float friction = 0.9f;
        private float amount = 0f;

        void Update()
        {

            rippleMaterial.SetFloat("_Amount", amount);
            amount *= friction;
        }

        public void RippleEffect(Transform target, float maxAmount, float newfriction)
        {
            amount = maxAmount;
            friction = newfriction;

            Vector3 posInScreen = GetComponent<Camera>().WorldToScreenPoint(target.position);

            rippleMaterial.SetFloat("_CenterX", posInScreen.x);
            rippleMaterial.SetFloat("_CenterY", posInScreen.y);
        }

        void OnRenderImage(RenderTexture src, RenderTexture dst)
        {
            Graphics.Blit(src, dst, rippleMaterial);
        }

    }
}