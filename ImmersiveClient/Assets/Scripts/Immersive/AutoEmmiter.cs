using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoEmmiter : MonoBehaviour {

    [SerializeField]
    public float ShootInterval = 3f;

    [SerializeField]
    public int MaxNum = 5;


    private float m_TimeCount = 0f;
    private Emmiter m_Emmiter = null;
    private int m_ShootCount = 0;

    private bool m_bShoot = true;

	// Use this for initialization
	void Start () {
        m_Emmiter = gameObject.GetComponent<Emmiter>();
        if (m_Emmiter == null)
        {
            CommonUtil.CommonLogger.LogError(string.Format("{0} autoemmiter has no emmit", gameObject.name));
        }
        m_TimeCount = ShootInterval;
        m_Emmiter.SetColorLerp(1f);
	}
	
	// Update is called once per frame
	void Update () {
        if (m_Emmiter == null || !m_bShoot)
            return;
        
        m_TimeCount += Time.deltaTime;
        //m_Emmiter.SetColorLerp(m_TimeCount / ShootInterval);
        if (m_TimeCount >= ShootInterval)
        {
            m_TimeCount -= ShootInterval;
            m_Emmiter.ReleaseLight(1f);
            m_ShootCount++;
        }

        if (MaxNum > 0)
        {
            m_Emmiter.SetColorLerp(1f - (float)m_ShootCount / MaxNum);
            if (m_ShootCount >= MaxNum)
            {
                m_bShoot = false;
                DimLight dl = gameObject.transform.parent.gameObject.GetComponentInChildren<DimLight>();
                if (dl != null)
                    dl.Dim();
            }
        }
	}
}
