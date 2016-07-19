using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CommonUtil
{
	public delegate bool TaskFunc();
	
	public class TaskMng : MonoBehaviour
	{
		#region static
		static TaskMng _instance;
		public static TaskMng Instance
		{
			get {
				if (_instance == null) {
					GameObject go = new GameObject ("CoroutineObject");
					GameObject.DontDestroyOnLoad (go);
					_instance = go.AddComponent<TaskMng> ();
				}
				return _instance;
			}
		}
		#endregion

		List<TaskObj> m_SpareTasks = new List<TaskObj>();
		List<TaskObj> m_BusyTasks = new List<TaskObj>();

		private TaskMng()
		{
			
		}

		List<TaskObj> m_FinishedTasks = new List<TaskObj>();
		void Update()
		{
			foreach (TaskObj o in m_BusyTasks) {
				if (o.Finished) {
					o.TaskOver ();
					m_FinishedTasks.Add (o);
				}
			}

			foreach (TaskObj o in m_FinishedTasks) {
				m_BusyTasks.Remove (o);
				m_SpareTasks.Add (o);
			}

			m_FinishedTasks.Clear ();
		}

		public void StartTreadTask(TaskFunc fun, FinCB cb = null, object param = null)
		{
			TaskObj t = GetTask ();
			t.Start (fun, cb, param);

			m_BusyTasks.Add (t);
		}

		TaskObj GetTask()
		{
			TaskObj ret = null;
			if (m_SpareTasks.Count > 0) {
				ret = m_SpareTasks [0];
				m_SpareTasks.RemoveAt (0);
			} else {
				ret = new TaskObj ();
			}

			return ret;
		}
	}
}