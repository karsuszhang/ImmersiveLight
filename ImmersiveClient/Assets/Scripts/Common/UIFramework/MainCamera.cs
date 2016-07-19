using UnityEngine;
using System.Collections;

namespace CommonUtil
{
	public class MainCamera : MonoBehaviour {

		void Awake()
		{
			UIManager.Instance.MainCamera = this.gameObject.GetComponent<Camera>();
			UIManager.Instance.MainCameraHolder = this;
		}
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public bool TapOnCollider(Collider cd, Vector3 screen_pos)
		{
			Camera ca = gameObject.GetComponent<Camera>();
			Ray r = ca.ScreenPointToRay (screen_pos);
			RaycastHit hitinfo = new RaycastHit ();

			return cd.Raycast (r,out hitinfo, 1000f);
		}
	}
}