using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AreaLightGenerator : MonoBehaviour {
    [SerializeField]
    public float Radius = 3f;

    [SerializeField]
    public int GenNum = 10;

    [SerializeField]
    public int MaxNumSameTime = 5;

    [SerializeField]
    public float GenLightIntensity = 0.6f;

    [SerializeField]
    public Color LightColor = Color.white;

    public DimLight BindedLight;

    const float GenInterval = 2f;

    private List<LightPlus> m_Lights = new List<LightPlus>();
    private int m_GenedNum = 0;
    private bool m_Stop = false;
    private float m_TimeCout = 0f;
	// Use this for initialization
	void Start () {
        GenLight(true);
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < m_Lights.Count; )
        {
            if (m_Lights[i].Released)
                m_Lights.RemoveAt(i);
            else
                i++;
        }

        m_TimeCout += Time.deltaTime;
        if (m_TimeCout >= GenInterval)
        {
            m_TimeCout -= GenInterval;
            if (m_Lights.Count < MaxNumSameTime && m_GenedNum < GenNum)
                GenLight();
        }

        if (!m_Stop)
        {
            if (m_Lights.Count == 0 && m_GenedNum >= GenNum)
            {
                m_Stop = true;
                if (BindedLight != null)
                    BindedLight.Dim();
                else
                    CommonUtil.Logger.LogError(gameObject.name + " has no binded light");
            }
        }
	}

    void OnDrawGizmosSelected()
    {
        #if UNITY_EDITOR
        Gizmos.color = Color.blue;
        //Gizmos.DrawSphere(gameObject.transform.position, 0.2f);
        Gizmos.DrawWireSphere(gameObject.transform.position, this.Radius);
        #endif
    }

    void GenLight(bool gen2full = false)
    {
        while (m_Lights.Count < MaxNumSameTime)
        {
            LightPlus lp = LightPlus.GenLightPlus();
            Vector3 pos = Vector3.zero;
            bool pos_ok = true;
            do
            {
                Vector3 dir = GameHelper.RandomNormalizedVector3();
                dir.y = 0f;
                dir.Normalize();
                pos = dir * GameHelper.Random(0f, Radius) + this.transform.position;
                Collider[] overs = Physics.OverlapSphere(pos, lp.RadiusLength);
                if(overs != null && overs.Length > 0)
                    pos_ok = false;
                else
                    pos_ok = true;
            } while(!pos_ok);

            //CommonUtil.Logger.Log(dir.ToString());
            lp.SetAbsolutePos(pos);
            lp.SetColor(LightColor, GenLightIntensity);
            m_Lights.Add(lp);

            m_GenedNum++;
            if (m_GenedNum >= MaxNumSameTime)
                break;
            if (!gen2full)
                break;
        }
    }
}
