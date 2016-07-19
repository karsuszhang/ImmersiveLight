using UnityEngine;
using System.Collections;

public class DistinguishBlock : BaseCDObj {

    [SerializeField]
    public Color TargetColor = Color.white;

    [SerializeField]
    public bool ReBirth = false;

    [SerializeField]
    public float BirthTime = 5f;

    private bool Distinguished = false;
    private float m_TimeCount = 0f;
    public DistinguishBlock() : base(ObjectType.DistinguishBlock)
    {
        
    }

    protected override void _Start()
    {
        base._Start();
        GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", TargetColor);
    }

    void LateUpdate()
    {
        if (ReBirth && Distinguished)
        {
            m_TimeCount -= Time.deltaTime;
            if (m_TimeCount <= 0f)
            {
                Distinguished = false;
                GetComponentInChildren<MeshRenderer>().enabled = true;
            }
        }
    }

    public override void CheckCD(BaseCDObj c)
    {
        if (Distinguished)
            return;
        
        if (c.Type == ObjectType.LightPlus)
        {
            RaycastHit final = FindCollideWithLightPlus(c as LightPlus);

            if (final.collider != null)
            {
                LightPlus lp = c as LightPlus;
                lp.EndAt(final.point, this);

                if (lp.LightColor == this.TargetColor || this.TargetColor == Color.white)
                {
                    Distinguish();
                }
            }
        }
    }

    private void Distinguish()
    {
        Distinguished = true;
        GetComponentInChildren<MeshRenderer>().enabled = false;
        m_TimeCount = BirthTime;
    }
}
