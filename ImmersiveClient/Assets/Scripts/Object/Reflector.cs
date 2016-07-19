using UnityEngine;
using System.Collections;

public class Reflector : BaseCDObj 
{
    [SerializeField]
    public bool DoubleSideReflect = false;

    [SerializeField]
    public bool ChangeColor = false;

    [SerializeField]
    public Color ReflectColor = Color.white;

    public Reflector() : base(ObjectType.Reflector)
    {
        
    }

    protected override void _Start()
    {
        base._Start();
        if (ChangeColor)
            GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", ReflectColor);
    }

    public override void CheckCD(BaseCDObj c)
    {
        if (c.Type == ObjectType.LightPlus)
        {
            RaycastHit final = FindCollideWithLightPlus(c as LightPlus);

            if (final.collider != null)
            {
                if (!DoubleSideReflect)
                {
                    if (Vector3.Dot(final.normal, this.Dir) < 0)
                    {
                        (c as LightPlus).EndAt(final.point, this);
                        return;
                    }
                   
                }
                LightPlus old = (c as LightPlus);
                old.EndAt(final.point, this);
                Vector3 org_dir = old.Dir;
                float angle = Mathf.Acos(Vector3.Dot(final.normal, org_dir));
                //CommonUtil.CommonLogger.Log("org cos " + angle + " deg " + Mathf.Rad2Deg * angle);
                float d = Vector3.Cross(org_dir, final.normal).y;
                //CommonUtil.CommonLogger.Log("cross y " + d);

                LightPlus rl = LightPlus.GenLightPlus();
                rl.StartAt(final.point, old.RadiusLength);
                rl.Dir = final.normal;
                rl.Speed = old.Speed;
                rl.SetColor(this.ChangeColor ? this.ReflectColor : old.LightColor, old.LightIntensity);
                rl.Scale = old.Scale;
                //CommonUtil.CommonLogger.Log("Reflect old scale " + old.Scale + " re " + rl.Scale);

                rl.gameObject.transform.RotateAround(rl.Pos, Vector3.up, (180f - Mathf.Rad2Deg * angle) * (d < 0 ? 1f : -1f));
                rl.AddUnCollideObj(this);
            }
        }
    }
}
