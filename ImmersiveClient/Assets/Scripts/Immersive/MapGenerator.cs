using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator {

    private List<MapNode> m_AliveNodes = new List<MapNode>();
    private List<MapNode> m_FinishedNodes = new List<MapNode>();
    private List<GameObject> m_LightTransfers = new List<GameObject>();

    public void StartLevel()
    {
        if (Game.Instance.CurPlayer == null)
        {
            CommonUtil.Logger.LogError("Can't find CurPlayer");
            return;
        }

        GameObject startnode = (CommonUtil.ResourceMng.Instance.GetResource("MapNode/MapNode2", CommonUtil.ResourceType.Model)) as GameObject;
        startnode.transform.position = GameObject.Find("StartPoint").transform.position;
        startnode.GetComponent<MapNode>().GenGuideLight(Game.Instance.CurPlayer.Pos);
        MapNode mn = startnode.GetComponent<MapNode>();
        mn.ParentPos = Game.Instance.CurPlayer.Pos;
        m_AliveNodes.Add(mn);
        mn.EventOnMapNodeFin += this.OnNodeFinished;
        //mn.GenChlidNode();
    }

    public void Update()
    {
        for (int i = 0; i < m_FinishedNodes.Count ;)
        {
            if ((m_FinishedNodes[i].transform.position - Game.Instance.CurPlayer.Pos).magnitude >= Constant.SafeDestroyNodeDistance)
            {
                GameObject.Destroy(m_FinishedNodes[i].gameObject);
                m_FinishedNodes.RemoveAt(i);
            }
            else
                i++;
        }
    }

    void OnNodeFinished(MapNode mn)
    {
        m_AliveNodes.Remove(mn);
        m_FinishedNodes.Add(mn);
        mn.EventOnMapNodeFin -= this.OnNodeFinished;
        //GenMapNode(mn.gameObject.transform.position);
        mn.GenChlidNode();
    }

    public MapNode GenMapNode(Vector3 ref_pos, Vector3 parent_pos)
    {
        Vector3 dir = parent_pos - ref_pos;
        dir.Normalize();

        Vector3 pos =  Game.Instance.GetNodeIntervalDis(dir) * dir + ref_pos;
        int mapnode = GameHelper.Random(1, 3);

        GameObject startnode = (CommonUtil.ResourceMng.Instance.GetResource("MapNode/MapNode" + mapnode, CommonUtil.ResourceType.Model)) as GameObject;
        startnode.transform.position = pos;

        CommonUtil.Logger.Log(startnode.transform.position.ToString());
        float angle = GameHelper.Random(90f, 270f);
        CommonUtil.Logger.Log("Turn angle " + angle);
        startnode.transform.RotateAround(ref_pos, Vector3.up, angle);
        CommonUtil.Logger.Log(startnode.transform.position.ToString());

        startnode.GetComponent<MapNode>().GenGuideLight(ref_pos);
        MapNode mn = startnode.GetComponent<MapNode>();
        mn.ParentPos = ref_pos;
        m_AliveNodes.Add(mn);
        mn.EventOnMapNodeFin += this.OnNodeFinished;

        if (Game.Instance.IsGenTransferStation())
        {
            Vector3 center_pos = ref_pos + 0.5f * (startnode.transform.position - ref_pos);
            float r_dis = GameHelper.Random(8f, 15f);
            Vector3 d = Vector3.Cross(Vector3.up, center_pos - ref_pos).normalized;
            float r_d = GameHelper.Random(-1f, 1f);
            Vector3 final_pos;
            if (r_d >= 0)
                final_pos = center_pos + r_dis * d;
            else
                final_pos = center_pos - r_dis * d;

            bool can_gen = true;
            float safe_threshold = 8f;
            foreach (MapNode m in m_AliveNodes)
            {
                if ((final_pos - m.transform.position).magnitude <= safe_threshold)
                    can_gen = false;
            }

            foreach (MapNode m in m_FinishedNodes)
            {
                if ((final_pos - m.transform.position).magnitude <= safe_threshold)
                    can_gen = false;
            }

            if (can_gen)
            {
                CommonUtil.Logger.Log("Gen Transfer at " + final_pos.ToString());
                GameObject transfer_node = CommonUtil.ResourceMng.Instance.GetResource("Object/LightTransfer", CommonUtil.ResourceType.Model) as GameObject;
                transfer_node.transform.position = final_pos;
                m_LightTransfers.Add(transfer_node);
            }
        }
            
        return mn;
    }
}
