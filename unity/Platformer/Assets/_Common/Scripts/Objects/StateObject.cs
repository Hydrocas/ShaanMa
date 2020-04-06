///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 22/10/2019 17:44
///-----------------------------------------------------------------

using System;
using UnityEngine;

namespace Com.IsartDigital.Common.Objects {
	public abstract class StateObject : GameplayObject {

        public Action DoAction;

        override public void Init () {
            SetModeVoid();
		}

        virtual public void SetModeVoid()
        {
            DoAction = DoActionVoid;
        }

        protected void DoActionVoid()
        {

        }
	}
}