using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

    public static Game Instance = null;

    public List<BaseCDObj> AllObjs
    {
        get
        {
            return m_CDObjs;
        }
    }
    private List<BaseCDObj> m_CDObjs = new List<BaseCDObj>();

    private Dictionary<Receiver, bool> m_LevelStatus = new Dictionary<Receiver, bool>();
	// Use this for initialization
	void Awake () {
        Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RegCDObj(BaseCDObj o)
    {
        if (m_CDObjs.Contains(o))
        {
            CommonUtil.CommonLogger.LogError("Obj Reg Twice: " + o.gameObject.name);
            return;
        }

        m_CDObjs.Add(o);
        m_CDObjs.Sort(this.SortCDObj);
    }

    int SortCDObj(BaseCDObj left, BaseCDObj right)
    {
        if(left.Type < right.Type)
            return -1;
        else if(left.Type == right.Type)
            return 0;
        else 
            return 1;
    }

    public void UnRegObject(BaseCDObj o)
    {
        //CommonUtil.CommonLogger.Log("Object UnReg " + o.gameObject.name);
        m_CDObjs.Remove(o);
    }

    public void CheckCD(BaseCDObj o, List<BaseCDObj> uncolliders = null)
    {
        foreach (BaseCDObj obj in m_CDObjs)
        {
            if (uncolliders != null && uncolliders.Contains(obj))
                continue;
            
            if (!o.Released && o != obj)
            {
                obj.CheckCD(o);
            }
        }
    }

    public void LevelComplete()
    {
        GameObject button = CommonUtil.UIManager.Instance.AddUI("UI/BackButton");
        button.GetComponent<UIButton>().onClick.Add(new EventDelegate(this.Back2Main));
    }

    public void Back2Main()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    public void RegLevelReceiver(Receiver r)
    {
        m_LevelStatus[r] = false;
    }

    public void ReceiverComplete(Receiver r)
    {
        if (!m_LevelStatus.ContainsKey(r))
        {
            CommonUtil.CommonLogger.LogError("Complete UnReg Receiver " + r.gameObject.name);
            return;
        }
           
        m_LevelStatus[r] = true;

        bool finished = true;
        foreach (var obj in m_LevelStatus)
        {
            if (!obj.Value)
                finished = false;
        }

        if (finished)
            LevelComplete();
    }
}
