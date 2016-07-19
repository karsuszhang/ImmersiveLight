using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

    private GameObject m_FollowObj = null;
    private Vector3 m_LastPos;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (m_FollowObj != null)
        {
            Vector3 dis = m_FollowObj.gameObject.transform.position - m_LastPos;
            //if (dis.magnitude > 0.1f)
            {
                gameObject.transform.position += dis;
                m_LastPos = m_FollowObj.gameObject.transform.position;
            }
        }
	
	}

    public void RegFollowObj(GameObject obj)
    {
        m_FollowObj = obj;
        m_LastPos = obj.transform.position;
    }
}
