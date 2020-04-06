///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 11/11/2019 10:59
///-----------------------------------------------------------------

using Com.IsartDigital.Common.Objects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Common.Managers {
	public class ScreensDisplayer {

        private List<ScreenObject> displayedScreens;

        private ScreenObject nextScreenToAppear;
        private Action callback;
        private ScreenObject transitionScreen;
        private int counter = 0;

        public ScreenObject[] DisplayedScreens => displayedScreens.ToArray();

        public ScreensDisplayer(Canvas[] canvas)
        {
            for (int i = canvas.Length - 1; i >= 0; i--)
            {
                SetCanvas(canvas[i]);
            }

            displayedScreens = new List<ScreenObject>();
        }

        public void SetCanvas(Canvas canvas)
        {
            ScreenObject[] screensObjects = canvas.GetComponentsInChildren<ScreenObject>(true);
            ScreenObject screenObject;

            for (int i = screensObjects.Length - 1; i >= 0; i--)
            {
                screenObject = screensObjects[i];
                screenObject.gameObject.SetActive(true);
                screenObject.Init(this);
                screenObject.OnDisappearEnd += SortScreens;
            }
        }

        #region Display

        public void Display(ScreenObject screenObject)
        {
            if (displayedScreens.Contains(screenObject)) return;

            displayedScreens.Add(screenObject);
            screenObject.Appear();
            SortScreens();
        }

        public void ForceDisplay(ScreenObject screenObject)
        {
            if (displayedScreens.Contains(screenObject)) return;

            displayedScreens.Add(screenObject);
            screenObject.ForceAppear();
            SortScreens();
        }

        #endregion

        #region Remove

        public void Remove(ScreenObject screenObject, Action callback)
        {
            this.callback = callback;
            screenObject.OnDisappearEnd += ScreenObject_OnDisappearEnd;
            Remove(screenObject);
        }

        public void Remove(ScreenObject screenObject)
        {
            displayedScreens.Remove(screenObject);
            screenObject.Disappear();
        }

        public void ForceRemove(ScreenObject screenObject)
        {
            displayedScreens.Remove(screenObject);
            screenObject.ForceDisappear();
        }

        public void RemoveAll()
        {
            for (int i = displayedScreens.Count - 1; i >= 0; i--)
            {
                Remove(displayedScreens[i]);
            }
        }

        public void ForceRemoveAll()
        {
            for (int i = displayedScreens.Count - 1; i >= 0; i--)
            {
                ForceRemove(displayedScreens[i]);
            }
        }

        public void RemoveAt(int index)
        {
            Remove(displayedScreens[index]);
        }

        public void ForceRemoveAt(int index)
        {
            ForceRemove(displayedScreens[index]);
        }

        #endregion

        #region Sort

        private void SortScreens()
        {
            int length = displayedScreens.Count - 1;

            if (length < 0) return;

            int i;
            ScreenObject screenObject;

            for (i = 0; i < length; i++)
            {
                screenObject = displayedScreens[i];
                screenObject.Interectable = false;
                screenObject.transform.SetSiblingIndex(i);
            }

            screenObject = displayedScreens[i];
            screenObject.Interectable = true;
            screenObject.transform.SetSiblingIndex(i);
        }

        private void SortScreens(ScreenObject sender)
        {
            SortScreens();
        }

        #endregion

        #region Transition

        public void DisplayTransition(ScreenObject transitionScreen, Action callback)
        {
            this.callback = callback;
            transitionScreen.OnAppearEnd += TransitionScreen_OnAppearEnd;
            transitionScreen.Appear();
        }

        private void TransitionScreen_OnAppearEnd(ScreenObject sender)
        {
            sender.OnAppearEnd -= TransitionScreen_OnAppearEnd;
            transitionScreen = sender;

            callback();
            callback = null;
        }

        public void RemoveTransition(Action callback = null)
        {
            if (transitionScreen == null) return;

            if(callback != null)
            {
                this.callback = callback;
                transitionScreen.OnDisappearEnd += TransitionScreen_OnDisappearEnd;
            }

            transitionScreen.Disappear();
        }

        private void TransitionScreen_OnDisappearEnd(ScreenObject sender)
        {
            sender.OnDisappearEnd -= TransitionScreen_OnDisappearEnd;
            transitionScreen = null;

            callback();
            callback = null;
        }

        #endregion

        #region Switch

        public void Switch(ScreenObject nextScreen, ScreenObject lastScreen)
        {
            if (displayedScreens.Contains(nextScreen)) return;

            nextScreenToAppear = nextScreen;
            lastScreen.OnDisappearEnd += ScreenObject_OnDisappearEnd;
            Remove(lastScreen);
        }

        public void Switch(ScreenObject nextScreen)
        {
            if (displayedScreens.Contains(nextScreen)) return;

            if(displayedScreens.Count == 0)
            {
                Display(nextScreen);
                return;
            }

            callback = DisplayNextScreen;
            nextScreenToAppear = nextScreen;
            ScreenObject lastScreen;

            for (int i = displayedScreens.Count - 1; i >= 0; i--)
            {
                lastScreen = displayedScreens[0];
                counter++;

                lastScreen.OnDisappearEnd += ScreenObject_OnDisappearEnd;
                Remove(lastScreen);
            }
        }

        private void ScreenObject_OnDisappearEnd(ScreenObject sender)
        {
            sender.OnDisappearEnd -= ScreenObject_OnDisappearEnd;

            if(counter > 0)
            {
                counter--;

                if (counter != 0) return;
            }

            callback();
            callback = null;
        }

        private void DisplayNextScreen()
        {
            Display(nextScreenToAppear);
            nextScreenToAppear = null;
        }

        #endregion
    }
}