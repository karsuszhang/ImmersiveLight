using UnityEngine;
using System.Collections;

public class Block : BaseCDObj {

    [SerializeField]
    public float BlockThreshold = 1f;

    [SerializeField]
    public int MinReflectNum = 2;

    [SerializeField]
    public int MaxReflectNum = 5;

    [SerializeField]
    public bool AbsorbEnergy = false;

    [SerializeField]
    public float AbsorbRate = 0.2f;

    private const float MinLightIntensity = 0.1f;

    public Block() : base(ObjectType.Block)
    {
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
                LightPlus lp = (c as LightPlus);
                lp.EndAt(final.point, this);
                if (lp.LightIntensity >= 1f)
                {
                    float absorb_rate = AbsorbEnergy ? (1f - AbsorbRate) : 1f;
                    int lightnum = GameHelper.Random(MinReflectNum, MaxReflectNum + 1);
                    float total_intensity = lp.LightIntensity * absorb_rate;
                    float intentsity4cal = total_intensity;
                    float angle = Mathf.Acos(Vector3.Dot(final.normal, lp.Dir));
                    float d = Vector3.Cross(lp.Dir, final.normal).y;


                    for (int i = 0; i < lightnum; i++)
                    {
                        float intensity = GameHelper.Random(MinLightIntensity, intentsity4cal - MinLightIntensity * (lightnum - i - 1)); 
                        if (i == lightnum - 1)
                            intensity = intentsity4cal;
                        intentsity4cal -= intensity;

                        float ratio = intensity / total_intensity * absorb_rate;
                        LightPlus rl = LightPlus.GenLightPlus();
                        rl.StartAt(final.point, lp.RadiusLength);
                        rl.Dir = -lp.Dir;
                        rl.Speed = Mathf.Max(1.5f, lp.Speed * ratio);//3f;
                        //rl.SetColor(HSBColor.LerpWithMinRatio(Color.black, lp.LightColor, ratio), intensity);
                        rl.SetColor(lp.LightColor, intensity);
                        rl.SetScaleRatio(ratio * lp.Scale, true);

                        float max_angle = (270f - Mathf.Rad2Deg * angle);
                        float a2t = GameHelper.Random(0f, max_angle);

                        rl.gameObject.transform.RotateAround(rl.Pos, Vector3.up, (a2t) * (d < 0 ? 1f : -1f));

                        rl.gameObject.name = lp.gameObject.name + "_sat_" + i;

                        rl.AddUnCollideObj(this);
                    }
                }
            }
        }
    }
}
