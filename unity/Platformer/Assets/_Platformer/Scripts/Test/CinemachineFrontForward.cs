///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 20/01/2020 12:34
///-----------------------------------------------------------------

using UnityEngine;
using Cinemachine;

namespace Com.Isartdigital.Platformer.Test {
	public class CinemachineFrontForward : MonoBehaviour
	{

		[SerializeField] CinemachineVirtualCamera virtualCamera;
		private CinemachineFramingTransposer framingTransposer;
		private Transform follow;

		private void Start () {
			/*
			foreach(CinemachineComponentBase cinemachineComponentBase in virtualCamera.GetComponentPipeline())
			{
				Debug.Log(cinemachineComponentBase);
			}
			*/
			//GetCinemachineComponent<CinemachinePipeline>();

			framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
			follow = virtualCamera.Follow;
		}
		
		private void Update () {
			Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(framingTransposer.m_ScreenX - framingTransposer.m_DeadZoneWidth, 0, 0));
			Debug.Log(pos);
			Debug.Log("follow : " + follow.position);

			if(follow.position.x < pos.x)
			{
				framingTransposer.m_ScreenX = 0.6f;
			}
			/*
			else if(Input.GetAxis("Horizontal") < 0)
			{
				framingTransposer.m_ScreenX = 0.75f;
			}
			*/
		}
	}
}