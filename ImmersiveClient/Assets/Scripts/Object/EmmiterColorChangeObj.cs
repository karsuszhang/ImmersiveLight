using UnityEngine;
using System.Collections;

public class EmmiterColorChangeObj : BaseCDObj {
    [SerializeField]
    public Color ChangeColor = Color.white;

    public ColorChangerGenerator Generator { private get; set;}
    public EmmiterColorChangeObj() : base(ObjectType.EmmiterColorChanger)
    {
    }

    protected override void _Start()
    {
        base._Start();
        GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", ChangeColor);
    }

    public void SetColor(Color r)
    {
        ChangeColor = r;
        GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", ChangeColor);
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
                this.Release();

                if (lp.LightEmmiter != null)
                {
                    lp.LightEmmiter.SetBaseColor(ChangeColor);
                }

                if (Generator != null)
                {
                    Generator.GenedChangerReleased(this);
                }
            }
        }
    }
}
