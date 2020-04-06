///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 21/01/2020 13:55
///-----------------------------------------------------------------

using Com.Isartdigital.Platformer.Data;
using Com.IsartDigital.Common.Objects;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Isartdigital.Platformer.UI {
	public class LeaderBoard : PopupObject {

		[SerializeField] private GameObject[] usernameContainers;

		private Text[] texts;

		private void Awake()
		{
			texts = new Text[usernameContainers.Length];

			for (int i = usernameContainers.Length - 1; i >= 0; i--)
			{
				texts[i] = usernameContainers[i].GetComponentInChildren<Text>();
			}
		}

		public void SetList(User[] users)
		{
			int usersLenght = users.Length;
			int textsLenght = texts.Length;

			for (int i = 0; i < textsLenght; i++)
			{
				if (i >= usersLenght)
				{
					usernameContainers[i].SetActive(false);
					continue;
				}

				if (!usernameContainers[i].activeSelf)
				{
					usernameContainers[i].SetActive(true);
				}

				texts[i].text = users[i].username;
			}
		}
	}
}