using UnityEngine;
using System.Collections;

public delegate void OnReceiverDestRatioChanged(float ratio, Receiver obj);

public class Receiver : BaseCDObj {

    [SerializeField]
    public float DestIntensity = 3;

    [SerializeField]
    public Color ReceiveColor = Color.white;

    [SerializeField]
    public float AbsorbRate = 0.2f;

    [SerializeField]
    public float DropRate = 0.001f;

    public event OnReceiverDestRatioChanged RatioChangeEvent;

    private float m_BaseIntensity;
    private float m_CurIntensity;
    private MeshRenderer m_Render = null;
    private Light m_Light = null;
    private bool Done = false;
    public Receiver() : base(ObjectType.Receiver)
    {
        
    }
	
    protected override void _Start()
    {
        base._Start();
        m_BaseIntensity = gameObject.GetComponentInChildren<Light>().intensity;
        m_CurIntensity = m_BaseIntensity;
        gameObject.GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", ReceiveColor);
        gameObject.GetComponentInChildren<Light>().color = ReceiveColor;
        m_Render = gameObject.GetComponentInChildren<MeshRenderer>();
        m_Light = gameObject.GetComponentInChildren<Light>();

        Game.Instance.RegLevelReceiver(this);
    }
	// Update is called once per frame
	void Update () {
        if (m_CurIntensity > m_BaseIntensity && !Done)
        {
            m_CurIntensity = Mathf.Max(m_BaseIntensity, m_CurIntensity - DropRate);
            SetRatio((m_CurIntensity - m_BaseIntensity) / DestIntensity);
            m_Light.intensity = m_CurIntensity;
        }
	}

    private void SetRatio(float r)
    {
        
        gameObject.GetComponentInChildren<MeshRenderer>().material.SetFloat("_Threshold", r);
        if (RatioChangeEvent != null)
            RatioChangeEvent(r, this);
    }

    public override void CheckCD(BaseCDObj c)
    {
        if (c.Type == ObjectType.LightPlus)
        {
            RaycastHit final = FindCollideWithLightPlus(c as LightPlus);

            if (final.collider != null)
            {
                LightPlus lp = (c as LightPlus);
                lp.EndAt(final.point, this);
                if (lp.LightColor == ReceiveColor && !Done)
                {
                    m_CurIntensity += lp.LightIntensity * AbsorbRate;
                    //CommonUtil.CommonLogger.Log(string.Format("{0} Cur {1} Dest {2}", gameObject.name, m_CurIntensity, DestIntensity));
                    if (m_CurIntensity >= DestIntensity)
                    {
                        Done = true;
                        Game.Instance.ReceiverComplete(this);
                    }

                    float ratio = Mathf.Min(1f, (m_CurIntensity - m_BaseIntensity) / (DestIntensity - m_BaseIntensity));

                    gameObject.GetComponentInChildren<Light>().intensity = m_CurIntensity;
                    SetRatio(ratio);
                }
                    
            }
        }
    }
}
