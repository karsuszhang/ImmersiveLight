using UnityEngine;
using System.Collections;

namespace CommonUtil
{
	public class MathTool{
		public static int Second2MilliSec(float sec)
		{
			float s = 1000f * sec;
			//CommonLogger.Log(string.Format("{0} round to {1}", s, Mathf.RoundToInt(s)));
			return Mathf.RoundToInt (s);
		}

	}
}