using UnityEngine;
using System.Collections;

public class LightTransfer : BaseCDObj {

    [SerializeField]
    public Stablizer[] Stabs;

    bool m_TransferLight = false;
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

    void TransferLight(bool trans)
    {
        m_TransferLight = trans;
        CommonUtil.Logger.Log("TransferLight " + trans);
    }
}
