using UnityEngine;
using System.Collections;

public class ImmersiveReceiver : BaseCDObj {

    public float IntensityDropRate = 0f;

    [SerializeField]
    public float MaxIntensity = 3;

    private Color m_Color = Color.white;
    private int m_CurCount = 0;
    private float m_BaseIntensity;
    public ImmersiveReceiver() : base(ObjectType.Receiver)
    {

    }

    protected override void _Start()
    {
        base._Start();
        m_BaseIntensity = gameObject.GetComponentInChildren<Light>().intensity;
        m_Color = gameObject.GetComponentInChildren<MeshRenderer>().material.GetColor("_Color");
    }
    // Update is called once per frame
    void Update () {

    }

    public override void CheckCD(BaseCDObj c)
    {
        if (c.Type == ObjectType.LightPlus)
        {
            RaycastHit final = FindCollideWithLightPlus(c as LightPlus);

            if (final.collider != null)
            {
                (c as LightPlus).EndAt(final.point, this);
                m_CurCount++;

                /*float ratio = (float)m_CurCount / DestCount;

                Color org_color = (c as LightPlus).LightColor;
                Color co = HSBColor.Lerp(m_Color, org_color, ratio);
                gameObject.GetComponentInChildren<Light>().color = org_color;
                gameObject.GetComponentInChildren<Light>().intensity = m_BaseIntensity + ratio * (DestIntensity - m_BaseIntensity);
                gameObject.GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", co);*/

            }
        }
    }
}
