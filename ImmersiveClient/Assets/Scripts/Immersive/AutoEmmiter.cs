using UnityEngine;
using System.Collections;

public class AutoEmmiter : MonoBehaviour {

    [SerializeField]
    public float ShootInterval = 3f;


    private float m_TimeCount = 0f;
    private Emmiter m_Emmiter = null;
	// Use this for initialization
	void Start () {
        m_Emmiter = gameObject.GetComponent<Emmiter>();
        if (m_Emmiter == null)
        {
            CommonUtil.CommonLogger.LogError(string.Format("{0} autoemmiter has no emmit", gameObject.name));
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (m_Emmiter == null)
            return;
        
        m_TimeCount += Time.deltaTime;
        m_Emmiter.SetColorLerp(m_TimeCount / ShootInterval);
        if (m_TimeCount >= ShootInterval)
        {
            m_TimeCount -= ShootInterval;
            m_Emmiter.ReleaseLight(1f);
        }
	}
}
