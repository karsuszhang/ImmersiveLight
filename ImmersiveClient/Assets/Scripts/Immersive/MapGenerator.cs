using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator {

    private List<MapNode> m_AliveNodes = new List<MapNode>();
    private List<MapNode> m_FinishedNodes = new List<MapNode>();

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
        m_AliveNodes.Add(mn);
        mn.EventOnMapNodeFin += this.OnNodeFinished;
        //mn.GenChlidNode();
    }

    public void Update()
    {
    }

    void OnNodeFinished(MapNode mn)
    {
        m_AliveNodes.Remove(mn);
        m_FinishedNodes.Add(mn);
        mn.EventOnMapNodeFin -= this.OnNodeFinished;
        //GenMapNode(mn.gameObject.transform.position);
        mn.GenChlidNode();
    }

    public MapNode GenMapNode(Vector3 ref_pos)
    {
        Vector3 dir = GameHelper.RandomNormalizedVector3();
        dir.y = 0f;
        dir.Normalize();

        Vector3 pos =  Game.Instance.GetNodeIntervalDis(dir) * dir + ref_pos;
        int mapnode = GameHelper.Random(1, 3);

        GameObject startnode = (CommonUtil.ResourceMng.Instance.GetResource("MapNode/MapNode" + mapnode, CommonUtil.ResourceType.Model)) as GameObject;
        startnode.transform.position = pos;
        startnode.GetComponent<MapNode>().GenGuideLight(ref_pos);
        MapNode mn = startnode.GetComponent<MapNode>();
        m_AliveNodes.Add(mn);
        mn.EventOnMapNodeFin += this.OnNodeFinished;
        return mn;
    }
}
