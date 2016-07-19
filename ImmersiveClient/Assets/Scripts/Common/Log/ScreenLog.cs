using UnityEngine;
using System.Collections;

public class ScreenLog : MonoBehaviour {

	// Use this for initialization
	void Start () {
		UITextList tl = gameObject.GetComponent<UITextList> ();
		foreach (CommonUtil.LogRecord r in CommonUtil.CommonLogger.s_Records) {
			tl.Add(TransOrgLog(r.type, r.log));
		}
	
		tl.scrollBar.value = 1f;
		CommonUtil.CommonLogger.NewLogEvent += this.LogIncoming;
	}

	void OnDestroy()
	{
		CommonUtil.CommonLogger.NewLogEvent -= LogIncoming;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LogIncoming(CommonUtil.LogType type, string log)
	{
		UITextList tl = gameObject.GetComponent<UITextList> ();
		tl.Add (TransOrgLog (type, log));
	}

	string TransOrgLog(CommonUtil.LogType type, string log)
	{
		string ret = log;
		switch (type) {
		case CommonUtil.LogType.Log:
			ret = (string.Format ("[FFFFFF]{0}", log));
			break;
		case CommonUtil.LogType.Warning:
			ret = (string.Format("[FFFF00]{0}", log));
			break;
		case CommonUtil.LogType.Error:
			ret = (string.Format ("[FF0000]{0}", log));
			break;
		}

		return ret;
	}
}
