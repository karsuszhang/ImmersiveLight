using UnityEngine;
using System.Collections;
using System.Threading;

namespace CommonUtil
{
	public delegate void FinCB(object o);
	public class TaskObj
	{
		Thread m_Thread = null;
		TaskFunc m_Func = null;
		FinCB m_cb = null;
		object m_Param = null;

		public bool Finished { get; private set;}
		public void Start (TaskFunc fun, FinCB cb = null, object param = null)
		{
			if (m_Thread == null)
				m_Thread = new Thread (TaskUpdate);

			m_Func = fun;
			m_cb = cb;
			m_Param = param;
			Finished = false;

			m_Thread.Start ();
		}

		void TaskUpdate()
		{
			while (!m_Func ()) {
			}

			Finished = true;
		}

		public void TaskOver()
		{
			if (m_cb != null)
				m_cb (m_Param);
		}
	}
}