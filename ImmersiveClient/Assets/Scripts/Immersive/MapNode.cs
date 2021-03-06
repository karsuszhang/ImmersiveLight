﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void MapNodeFinished(MapNode mn);

public class MapNode : MonoBehaviour {

    [SerializeField]
    public float ActivateDis = 10f;

    public event MapNodeFinished EventOnMapNodeFin;

    private bool Activated = false;

    Dictionary<DimLight, bool> m_LightDimState = new Dictionary<DimLight, bool>();

    List<LightPlus> m_GuideLP = new List<LightPlus>();
    List<GameObject> m_ObstacleObjs = new List<GameObject>();

    public Vector3 ParentPos;
    const int LinkChildNum = 1;
    private List<MapNode> m_LinkedChildNode = new List<MapNode>();

    EventPoint[] m_Events;
	// Use this for initialization
	void Start () {
        DimLight[] dls = GetComponentsInChildren<DimLight>();
        foreach (DimLight dl in dls)
        {
            m_LightDimState[dl] = false;
            dl.EventDimOver += this.OnLightDimed;
        }

        GenEvent();
	}

    void OnDestroy()
    {
        foreach (LightPlus lp in m_GuideLP)
        {
            if (!lp.Released)
                lp.Release();
        }
        m_GuideLP.Clear();

        foreach (GameObject go in m_ObstacleObjs)
        {
            GameObject.Destroy(go);
        }
        m_ObstacleObjs.Clear();
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
                    foreach (MapNode mn in m_LinkedChildNode)
                    {
                        //mn.GenChlidNode();
                    }
                }
            }
        }

        for (int i = 0; i < m_GuideLP.Count ;)
        {
            if (m_GuideLP[i].Released)
                m_GuideLP.RemoveAt(i);
            else
                i++;
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
        float light_interval = Game.Instance.GetGuideLightInterval();
        float dis = (transform.position - pos).magnitude;
        int guide_num = Mathf.CeilToInt(dis / light_interval);
        float interval = dis / guide_num;
        Vector3 f = (transform.position - pos).normalized;
        Vector3 r = Vector3.Cross(f, Vector3.up);
        CommonUtil.Logger.Log("GenGuideLight " + guide_num);
        for (int i = 0; i < guide_num; i++)
        {
            float disturb_f = GameHelper.Random(-1.5f, 1.5f);
            float disturb_h = GameHelper.Random(-3.5f, 3.5f);

            Vector3 p = pos + ((i + 1) * interval + disturb_f) * f + disturb_h * r;
            LightPlus lp = LightPlus.GenLightPlus();
            lp.SetAbsolutePos(p);
            lp.SetColor(lp.LightColor, 0.3f);
            //CommonUtil.Logger.Log("Gen Guide Light at " + p.ToString());
            if (i > 0)
            {
                if (Game.Instance.GenTrapOnRoad())
                {
                    float ratio = GameHelper.Random(0f, 1f);
                    Vector3 trap_pos = m_GuideLP[i - 1].Pos + ratio * (p - m_GuideLP[i-1].Pos).magnitude * (p - m_GuideLP[i-1].Pos).normalized;
                    GameObject trap = Game.Instance.GenTrap();
                    trap.transform.position = trap_pos;
                    m_ObstacleObjs.Add(trap);
                }

                if (Game.Instance.GenTrapOnRoad())
                {
                    GameObject enemy = Game.Instance.GenEnemy();
                    m_ObstacleObjs.Add(enemy);
                    Enemy e = enemy.GetComponent<Enemy>();

                    Vector3 dir = GameHelper.RandomNormalizedVector3(true);
                    enemy.transform.position = p + dir * GameHelper.Random(Mathf.Min(1f, e.AlarmDis), e.AlarmDis);
                   
                }
            }
            m_GuideLP.Add(lp);
        }
    }

    public void GenChlidNode()
    {
        if (m_LinkedChildNode.Count > 0)
            return;

        for (int i = 0; i < LinkChildNum; i++)
        {
            MapNode mn = Game.Instance.MapGen.GenMapNode(this.transform.position, ParentPos);
            //mn.GenGuideLight(this.transform.position);
            m_LinkedChildNode.Add(mn);
        }
    }

    void GenEvent()
    {
        m_Events = gameObject.GetComponentsInChildren<EventPoint>();
        if (m_Events.Length == 0)
            return;

        int num = Mathf.Min(m_Events.Length, Mathf.CeilToInt(GameHelper.Random(Game.Instance.GetEventNumMin(), 1f) * m_Events.Length));
        //CommonUtil.Logger.Log("Event Num " + num);
        if (num == 0)
            return;
        
        for (int i = 0; i < m_Events.Length; i++)
        {
            float possible = GameHelper.Random(0f, 1f);
            //CommonUtil.Logger.Log("possible " + possible);
            if (possible <= Game.Instance.GetEventGeneratePossible())
            {
                m_Events[i].GenEvent();
            }
        }
    }
}
