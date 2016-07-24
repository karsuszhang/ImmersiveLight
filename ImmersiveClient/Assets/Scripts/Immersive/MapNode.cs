using UnityEngine;
using System.Collections;

public class MapNode : MonoBehaviour {

    [SerializeField]
    public float ActivateDis = 10f;

    private bool Activated = false;
	// Use this for initialization
	void Start () {
        GenGuideLight();
	}
	
	// Update is called once per frame
	void Update () {
        if (!Activated)
        {
            if (Game.Instance.CurPlayer != null)
            {
                float dis = (this.transform.position - Game.Instance.CurPlayer.Pos).magnitude;
                if (dis <= ActivateDis)
                {
                    AutoEmmiter[] aes = gameObject.GetComponentsInChildren<AutoEmmiter>();
                    foreach (AutoEmmiter ae in aes)
                    {
                        ae.Activated = true;
                    }
                    Activated = true;
                }
            }
        }
	}

    void GenGuideLight()
    {
        if (Game.Instance.CurPlayer == null)
        {
            CommonUtil.Logger.LogError("Can't find CurPlayer on " + gameObject.name + " GenGuide");
            return;
        }

        float dis = (transform.position - Game.Instance.CurPlayer.Pos).magnitude;
        int guide_num = Mathf.CeilToInt(dis / 4f);
        Vector3 f = (transform.position - Game.Instance.CurPlayer.Pos).normalized;
        Vector3 r = Vector3.Cross(f, Vector3.up);
        for (int i = 0; i < guide_num; i++)
        {
            float disturb_f = GameHelper.Random(0f, 3.5f);
            float disturb_h = GameHelper.Random(-3.5f, 3.5f);

            Vector3 p = Game.Instance.CurPlayer.Pos + (i + 1 + disturb_f) * f + disturb_h * r;
            LightPlus lp = LightPlus.GenLightPlus();
            lp.SetAbsolutePos(p);
            CommonUtil.Logger.Log("Gen Guide Light at " + p.ToString());
        }
    }
}
