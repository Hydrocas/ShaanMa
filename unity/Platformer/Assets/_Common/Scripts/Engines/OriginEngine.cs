///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 23/10/2019 17:32
///-----------------------------------------------------------------
using UnityEngine;

namespace Com.IsartDigital.Common.Engines {
	public class OriginEngine {

        private GameObject model;
        private GameObject container;
        private Vector3 direction;
        private Vector3 modelSize;

        public OriginEngine(GameObject container, GameObject model)
        {
            this.container = container;
            this.model = model;

            modelSize = model.GetComponent<Renderer>().bounds.size;
        }

        public void Move(Vector3 direction)
        {
            this.direction = direction;
            Vector3 lastPos = model.transform.position;

            model.transform.localPosition = Vector3.Scale(-container.transform.InverseTransformVector(direction), modelSize / 2);
            container.transform.position += lastPos - model.transform.position;
        }

        public void UpdatePosition()
        {
            Move(direction);
        }

        public void Destroy()
        {
            container = null;
            model = null;
        }

    }
}