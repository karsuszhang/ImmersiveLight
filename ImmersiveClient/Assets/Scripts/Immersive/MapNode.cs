using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void MapNodeFinished(MapNode mn);

public class MapNode : MonoBehaviour {

    [SerializeField]
    public float ActivateDis = 10f;

    public event MapNodeFinished EventOnMapNodeFin;

    private bool Activated = false;

    Dictionary<DimLight, bool> m_LightDimState = new Dictionary<DimLight, bool>();
	// Use this for initialization
	void Start () {
        DimLight[] dls = GetComponentsInChildren<DimLight>();
        foreach (DimLight dl in dls)
        {
            m_LightDimState[dl] = false;
            dl.EventDimOver += this.OnLightDimed;
        }
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

    void OnLightDimed(DimLight dl)
    {
        if (m_LightDimState.ContainsKey(dl))
            m_LightDimState[dl] = true;

        bool all_dimed = true;
        foreach (var obj in m_LightDimState)
        {
            if (!obj.Value)
                all_dimed = false;
        }

        if (all_dimed && EventOnMapNodeFin != null)
            EventOnMapNodeFin(this);
            
    }

    public void GenGuideLight(Vector3 pos)
    {
        float dis = (transform.position - pos).magnitude;
        int guide_num = Mathf.CeilToInt(dis / 4f);
        Vector3 f = (transform.position - pos).normalized;
        Vector3 r = Vector3.Cross(f, Vector3.up);
        for (int i = 0; i < guide_num; i++)
        {
            float disturb_f = GameHelper.Random(0f, 3.5f);
            float disturb_h = GameHelper.Random(-3.5f, 3.5f);

            Vector3 p = pos + (i + 1 + disturb_f) * f + disturb_h * r;
            LightPlus lp = LightPlus.GenLightPlus();
            lp.SetAbsolutePos(p);
            //CommonUtil.Logger.Log("Gen Guide Light at " + p.ToString());
        }
    }
}
