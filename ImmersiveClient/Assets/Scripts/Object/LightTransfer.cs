using UnityEngine;
using System.Collections;

public class LightTransfer : BaseCDObj {

    [SerializeField]
    public Stablizer[] Stabs;

    bool m_TransferLight = false;

    float m_TimeCount = 0f;

    const float TransferInterval = 0.5f;

    public LightTransfer() : base(ObjectType.LightTransfer)
    {
    }


    public override void CheckCD(BaseCDObj c)
    {
        base.CheckCD(c);
    }

    protected override void _Start()
    {
        base._Start();
        foreach (Stablizer s in Stabs)
        {
            s.EventHoldPlayer += this.TransferLight;
        }
    }

    void LateUpdate()
    {
        if (m_TransferLight)
        {
            m_TimeCount += Time.deltaTime;
            if (m_TimeCount >= TransferInterval)
            {
                m_TimeCount -= TransferInterval;
                Game.Instance.TransferScore();
            }
        }
    }

    void TransferLight(bool trans)
    {
        m_TransferLight = trans;
        m_TimeCount = 0f;
        CommonUtil.Logger.Log("TransferLight " + trans);
    }
}
