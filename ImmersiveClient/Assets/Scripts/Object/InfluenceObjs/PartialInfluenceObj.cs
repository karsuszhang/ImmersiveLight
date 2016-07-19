using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartialInfluenceObj : ReceiverInfluenceObj {

    [SerializeField]
    public Receiver[] TotalInfluencer;

    Dictionary<Receiver, float> m_ReceiverProgress = new Dictionary<Receiver, float>();

    bool m_DoAni = false;
    const float ANI_TIME = 1.5f;
    float m_StartRatio = 0f;
    float m_EndRatio = 0f;
    float m_TimeCount = 0f;
	// Use this for initialization
	void Start () {
        if (TotalInfluencer != null)
        {
            foreach (Receiver r in TotalInfluencer)
            {
                r.RatioChangeEvent += this.OnRatioChanged;
                m_ReceiverProgress[r] = 0f;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (m_DoAni)
        {
            m_TimeCount += Time.deltaTime;
            float l = m_StartRatio + m_TimeCount / ANI_TIME * (m_EndRatio - m_StartRatio);
            gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Threshold", l);
            if (l >= m_EndRatio)
            {
                m_StartRatio = l;
                m_DoAni = false;
            }
        }
	}

    public override void OnRatioChanged(float ratio, Receiver obj)
    {
        if (!m_ReceiverProgress.ContainsKey(obj))
        {
            CommonUtil.CommonLogger.LogError(string.Format("PartialInfluenceObj {0} receive uninit event from {1}", gameObject.name, obj.gameObject.name));
            return;
        }

        m_ReceiverProgress[obj] = ratio;
        //CommonUtil.CommonLogger.Log(string.Format("PartialInfluenceObj {0} receive receiver {1} cur ratio {2}", gameObject.name, obj.name, ratio));
        if (ratio >= 1f)
        {
            float total_progress = 0f;
            foreach (var o in m_ReceiverProgress)
            {
                total_progress += o.Value / (float)m_ReceiverProgress.Count;
            }

            m_TimeCount = 0f;
            m_EndRatio = total_progress;
            m_DoAni = true;
            CommonUtil.CommonLogger.Log(string.Format("PartialIO {0} doAni from {1} to {2}", gameObject.name, m_StartRatio, m_EndRatio));
        }


        //if(ratio >= 1f)
            
        //gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Threshold", ratio);
    }
}
