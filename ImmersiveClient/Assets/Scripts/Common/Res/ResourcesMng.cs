using UnityEngine;
using System.Collections;

namespace CommonUtil
{
	public enum ResourceType
	{
		UI,
		Model,
		TXT,
		Effect,
	}
	public class ResourceMng {

		static public ResourceMng Instance
		{
			get {
				if (_instance == null)
					_instance = new ResourceMng ();

				return _instance;
			}
		}

		static ResourceMng _instance = null;

		private ResourceMng()
		{
		}

		public Object GetResource(string res, ResourceType type)
		{
			switch (type) {
			case ResourceType.UI:
			case ResourceType.Model:
			case ResourceType.Effect:
				{
					GameObject o = Resources.Load (res) as GameObject;
					if (o != null)
						return GameObject.Instantiate (o);
					break;
				}
			case ResourceType.TXT:
				{
					TextAsset t = Resources.Load (res) as TextAsset;
					if (t != null)
						return t;
					break;
				}
			}
			return null;
		}

		public void Release(Object res)
		{
			Object.Destroy (res);
		}
	}
}