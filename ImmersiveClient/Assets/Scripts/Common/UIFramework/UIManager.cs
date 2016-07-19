using UnityEngine;
using System.Collections;

namespace CommonUtil
{
	public class UIManager : MonoBehaviour {

		public static UIManager Instance
		{
			get {
				if (_instance == null) {
					ResourceMng.Instance.GetResource ("UI/UI Root", ResourceType.UI);

				}

				return _instance;
			}
		}

		protected static UIManager _instance = null;

		public UIRoot Root
		{
			get {
				return gameObject.GetComponent<UIRoot> ();
			}
		}

		public Camera CurUICamera
		{
			get{
				return gameObject.GetComponentInChildren<Camera> ();
			}
		}

		public Camera MainCamera;

		public MainCamera MainCameraHolder;

		void Awake()
		{
			_Awake ();
		}

		void OnDestroy()
		{
			_OnDestroy ();
		}
			
		protected virtual void _Awake()
		{
			_instance = this;
		}

		protected virtual void _OnDestroy()
		{
			_instance = null;
		}

		public virtual GameObject AddUI(string res, Transform parent = null)
		{
			GameObject sl = ResourceMng.Instance.GetResource(res, ResourceType.UI) as GameObject;
			Vector3 pos = sl.transform.localPosition;
			Vector3 scale = sl.transform.localScale;
			sl.transform.parent = parent == null ? this.transform : parent;
			sl.transform.localPosition = pos;
			sl.transform.localScale = scale;

			return sl;
		}

		public Vector3 GetScreenPos(Vector3 world_pos)
		{
			if (MainCamera == null) {
				CommonLogger.LogError ("Can't find MainCamera");
				return Vector3.zero;
			}

			Vector3 p1 = MainCamera.WorldToScreenPoint (world_pos);

			Vector3 pixel_by_center = p1 - new Vector3 (Screen.width / 2, Screen.height / 2, 0);

			float scale = (float)Screen.width / 720f;

			return pixel_by_center / scale;
		}
	}
}