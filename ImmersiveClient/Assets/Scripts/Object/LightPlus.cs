using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightPlus : BaseCDObj {

    enum RunningState
    {
        Flying,
        Ending,
        Starting,
    }

    public LightPlus() : base(ObjectType.LightPlus)
    {
        LightColor = Color.white;
    }

	public float Speed = 0f;

    public float RadiusLength
    {
        get
        {
            if (m_SCollider != null)
            {
                return this.gameObject.transform.localScale.x * m_SCollider.radius * m_SCollider.gameObject.transform.localScale.x;
            }
            return 0.1f;
        }
    }

    public float Scale 
    {
        get
        {
            return this.gameObject.transform.localScale.x;
        }
        set
        {
            this.gameObject.transform.localScale = new Vector3(value, value, value);
        }
    }
        
    public Color LightColor{get; private set;}
    public float LightIntensity{ get; private set; }

    public Emmiter LightEmmiter = null;

    private RunningState m_CurState = RunningState.Flying;
    //private Vector3 m_EndPos;
    //private Vector3 m_StartPos;
    private float m_DestLength;

    private float m_BaseScale = 1;
    private const float MinScale = 0.6f;

    private List<BaseCDObj> m_UnCollideObjs = new List<BaseCDObj>();

    private SphereCollider m_SCollider;
    public static LightPlus GenLightPlus()
    {
        GameObject lpo = CommonUtil.ResourceMng.Instance.GetResource("Object/LightPlus", CommonUtil.ResourceType.Model) as GameObject;
        LightPlus lo = lpo.GetComponent<LightPlus>();
        return lo;
    }

    protected override void _Start()
    {
        base._Start();
        m_SCollider = GetComponentInChildren<SphereCollider>();
        m_BaseScale = Scale;
    }

	// Update is called once per frame
	void LateUpdate () {
        if (m_CurState != RunningState.Starting)
        {
            if (Speed > 0.1f)
                this.Pos += Time.deltaTime * Speed * gameObject.transform.forward;
        }
        /*else
        {
            Length += (Time.deltaTime * Speed * gameObject.transform.forward).magnitude;
            if (Length >= m_DestLength)
            {
                Length = m_DestLength;
                m_CurState = RunningState.Flying;
            }
        }*/

        if (m_CurState == RunningState.Flying || m_CurState == RunningState.Starting)
        {
            Game.Instance.CheckCD(this, m_UnCollideObjs);
        }
        else if (m_CurState == RunningState.Ending)
        {
            DoEnding();
        }

        CheckBoard();
	}

    public void EndAt(Vector3 end_pos, BaseCDObj block_obj)
    {
        Release();
        //m_EndPos = end_pos;
        //m_CurState = RunningState.Ending;
    }

    public void StartAt(Vector3 start_pos, float dest_len)
    {
        Pos = start_pos;

        /*m_CurState = RunningState.Starting;
        m_DestLength = dest_len;

        Length = 0f;*/
        //RadiusLength = dest_len;
    }

    private void CheckBoard()
    {
        if (Mathf.Abs(Pos.x) > 10f || Mathf.Abs(Pos.z) > 40f)
            Release();
    }

    void DoEnding()
    {
        /*//CommonUtil.CommonLogger.Log(Vector3.Dot(Dir, (m_EndPos - Pos)).ToString());
        if (Vector3.Dot(Dir, (m_EndPos - Pos)) < 0)
        {
            Release();
            return;
        }

        this.Scale = Mathf.Max(0f, (m_EndPos - Pos).magnitude);

        if (RadiusLength < Vector3.kEpsilon)
            Release();*/
    }

    public void SetColor(Color c, float intensity)
    {
        //CommonUtil.CommonLogger.Log(gameObject.name + " SetColor " + c.ToString());
        MeshRenderer[] mrs = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in mrs)
        {
            if (mr.material != null)
            {
                mr.material.SetColor("_Color", c);
            }
        }

        Light[] ls = gameObject.GetComponentsInChildren<Light>();

        foreach (Light l in ls)
        {
            l.color = c;
            l.intensity = intensity;
        }

        LightIntensity = intensity;
        LightColor = c;
    }

    public void AddUnCollideObj(BaseCDObj o)
    {
        //CommonUtil.CommonLogger.Log(gameObject.name + " add uco : " + o.gameObject.name);
        m_UnCollideObjs.Add(o);
    }

    public void SetScaleRatio(float ratio, bool use_min_scale)
    {
        if (use_min_scale)
            Scale = MinScale + ratio * (m_BaseScale - MinScale);
        else
            Scale = m_BaseScale * ratio;
    }
}
