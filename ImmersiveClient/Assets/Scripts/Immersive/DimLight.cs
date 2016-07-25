using UnityEngine;
using System.Collections;

public delegate void LightDimOver(DimLight dl);

public class DimLight : MonoBehaviour {

    public event LightDimOver EventDimOver;

    private Light m_Light;
    private float m_OrgIntensity = 0f;
  
    const float BaseIntensity = 0.2f;
    const float DimTime = 3f;

    private bool IsDim = false;
    private float m_TimeCount = 0f;
	// Use this for initialization
	void Start () {
        m_Light = GetComponent<Light>();
        m_OrgIntensity = m_Light.intensity;
	}
	
	// Update is called once per frame
	void Update () {
        if (IsDim)
        {
            m_TimeCount += Time.deltaTime;
            float ratio = Mathf.Min(1f, m_TimeCount / DimTime);

            m_Light.intensity = BaseIntensity + (1f - ratio) * (m_OrgIntensity - BaseIntensity);
            if (m_TimeCount >= DimTime)
            {
                if (EventDimOver != null)
                    EventDimOver(this);
                IsDim = false;
            }
        }
	}

    public void Dim()
    {
        IsDim = true;
    }
}
