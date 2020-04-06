///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 05/11/2019 15:38
///-----------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Common.Engines {
    public class ColorEngine {

        private List<GameObject> models;
        private Color _color = Color.white;

		public ColorEngine(GameObject model)
        {
            models = new List<GameObject>() { model };
        }

        public ColorEngine(List<GameObject> models)
        {
            this.models = new List<GameObject>(models);
        }

        public Color Color {
            set 
            {
                _color = value;

                Renderer renderer;
                MaterialPropertyBlock propBlock;

                for (int i = 0; i < models.Count; i++)
                {
                    renderer = models[i].GetComponent<Renderer>();
                    propBlock = new MaterialPropertyBlock();

                    // Get the current value of the material properties in the renderer.
                    renderer.GetPropertyBlock(propBlock);

                    // Assign our new value.
                    propBlock.SetColor("_Color", value);

                    // Apply the edited values to the renderer.
                    renderer.SetPropertyBlock(propBlock);
                }

            }
            get
            {
                return _color;
            }
        }

        public void Destroy()
        {
            models = null;
        }
    }
}