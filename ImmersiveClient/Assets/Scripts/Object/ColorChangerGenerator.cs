using UnityEngine;
using System.Collections;

public class ColorChangerGenerator : MonoBehaviour {

    [SerializeField]
    public float GenChangerInterval = 5f;

    [SerializeField]
    public Color ChangerColor = Color.red;

    private EmmiterColorChangeObj Obj = null;
    private float m_TimeCount = 0f;
	// Use this for initialization
	void Start () {
        GenChanger();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (this.Obj == null)
        {
            m_TimeCount -= Time.deltaTime;
            if (m_TimeCount <= 0f)
                GenChanger();
        }
	}

    public void GenedChangerReleased(EmmiterColorChangeObj o)
    {
        if (o != Obj)
        {
            CommonUtil.CommonLogger.LogError(string.Format("Mismath ColorChanger {0} Release on {1}", o.gameObject.name, gameObject.name));
            return;
        }
        Obj = null;
        m_TimeCount = GenChangerInterval;
    }

    private void GenChanger()
    {
        GameObject obj = CommonUtil.ResourceMng.Instance.GetResource("Object/ColorChanger", CommonUtil.ResourceType.Model) as GameObject;
        EmmiterColorChangeObj changer = obj.GetComponent<EmmiterColorChangeObj>();
        changer.Pos = new Vector3(this.transform.position.x, 0f, this.transform.position.z);
        changer.SetColor(this.ChangerColor);
        changer.Generator = this;

        this.Obj = changer;
    }
}
