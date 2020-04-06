using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.IsartDigital.Common.Interface {
    public delegate void IBasicAnimatorEventHandler(IBasicAnimator sender);
    enum BasicAnimatorState { Appear, Disappear };
    public interface IBasicAnimator
    {
        void Appear();
        void EndAppear();
        void Disappear();
        void EndDisappear();
    }
}
