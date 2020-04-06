///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 20/03/2020 01:19
///-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Com.Isartdigital.Common.UI {
	public class SortingRendererText : MonoBehaviour {

        [SerializeField] private int sortingLayerID;
        [SerializeField] private int orderInLayer = 0;

        void Start()
        {
            Renderer renderer = GetComponent<Renderer>();

            renderer.sortingOrder = orderInLayer;
            renderer.sortingLayerID = sortingLayerID;
        }
    }
}