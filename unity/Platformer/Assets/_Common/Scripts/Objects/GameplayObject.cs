///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 06/11/2019 14:13
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Common.Objects {
	public abstract class GameplayObject : MonoBehaviour {
	
		virtual public void Init()
        {

        }

        virtual public void Destroy()
        {
            Destroy(gameObject);
            RemoveEvents();
        }

        virtual public void RemoveEvents()
        {

        }
	}
}