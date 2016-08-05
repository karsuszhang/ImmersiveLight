using UnityEngine;
using System.Collections;

enum PointEventType
{
    Enemy,
    Trap,
}

public class EventPoint : MonoBehaviour {

    [SerializeField]
    private PointEventType Type;

    private GameObject m_EventObj = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy()
    {
        if (m_EventObj != null)
            GameObject.Destroy(m_EventObj);
    }

    public void GenEvent()
    {
        if (Type == PointEventType.Enemy)
        {
            m_EventObj = CommonUtil.ResourceMng.Instance.GetResource("Object/Enemy", CommonUtil.ResourceType.Model) as GameObject;
        }
        else if (Type == PointEventType.Trap)
        {
            m_EventObj = CommonUtil.ResourceMng.Instance.GetResource("Object/Trap", CommonUtil.ResourceType.Model) as GameObject;
        }
        else
        {
            CommonUtil.Logger.LogError("Unknow Event " + Type.ToString());
            return;
        }

        m_EventObj.transform.position = this.gameObject.transform.position;
    }
}
