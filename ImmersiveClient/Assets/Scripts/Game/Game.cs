using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
    public Game()
    {
        CommonUtil.Logger.Log("Game Construct");
        _Instance = this;
    }

    public static Game Instance
    {
        get
        {
            return _Instance;

            /*if (_Instance == null)
                return GameObject.FindObjectOfType<Game>();
            else
                return _Instance;*/
        }
    }
    private static Game _Instance = null;

    public List<BaseCDObj> AllObjs
    {
        get
        {
            return m_CDObjs;
        }
    }
    private List<BaseCDObj> m_CDObjs = new List<BaseCDObj>();


    private InGameMainUI m_MainUI = null;
    private int m_CurScore = 0;

    public MapGenerator MapGen{get{ return m_MapGenerator; }}
    private MapGenerator m_MapGenerator = new MapGenerator();

    public ImmersiveReceiver CurPlayer { get; private set;}

    public float GameTime { get ; private set; }
	// Use this for initialization
	void Awake () {
        
        m_MainUI = CommonUtil.UIManager.Instance.AddUI("UI/InGamePanel").GetComponent<InGameMainUI>();
        GameTime = 0f;
	}

    void Start()
    {
        m_MapGenerator.StartLevel();
    }
	
	// Update is called once per frame
	void LateUpdate () {
        m_MapGenerator.Update();

        GameTime += Time.deltaTime;
	}

    public void RegCDObj(BaseCDObj o)
    {
        if (m_CDObjs.Contains(o))
        {
            CommonUtil.Logger.LogError("Obj Reg Twice: " + o.gameObject.name);
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
        //CommonUtil.Logger.Log("Object UnReg " + o.gameObject.name);
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

    public void Back2Main()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    public void AddScore(int s)
    {
        m_CurScore += s;
        m_MainUI.SetScore(m_CurScore);
    }

    public void RegCurPlayer(ImmersiveReceiver ir)
    {
        CurPlayer = ir;
        GameObject.FindObjectOfType<FollowCamera>().RegFollowObj(ir.gameObject);
        CommonUtil.Logger.Log("Immersive Player Change to " + ir.gameObject.name);
    }

    public void TransferScore()
    {
        m_CurScore--;
        PlayerInfo.Instance.ScoreInBag++;
    }

    public void GameOver()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    #region Game RunTime Diffculty
    const float GamePhase1 = Constant.SecondsOfMinute * 5f;
    const float GamePhase2 = Constant.SecondsOfMinute * 10f;

    public float GetEventNumMin()
    {
        return Mathf.Min(1f, GameTime / GamePhase1);
    }

    public float GetEventGeneratePossible()
    {
        if (GameTime < GamePhase1)
            return 0f;
        
        return Mathf.Min(1f, (GameTime - GamePhase1) / (GamePhase2 - GamePhase1));
    }

    public float GetNodeIntervalDis(Vector3 dir)
    {
        return GameHelper.Random(25f, 40f);
    }

    public float GetGuideLightInterval()
    {
        return 4f;
    }

    public bool GenTrapOnRoad()
    {
        float possible = GameHelper.Random(0f, 1f);
        float cur = 0.05f + Mathf.Min(0.45f, Mathf.Max(0f, (GameTime - GamePhase1) / (GamePhase2 - GamePhase1) * 0.45f));
        return possible <= cur;
    }

    public GameObject GenTrap()
    {
        return CommonUtil.ResourceMng.Instance.GetResource("Object/Trap", CommonUtil.ResourceType.Model) as GameObject;
    }

    public GameObject GenEnemy()
    {
        return CommonUtil.ResourceMng.Instance.GetResource("Object/Enemy", CommonUtil.ResourceType.Model) as GameObject;
    }

    public bool IsGenTransferStation()
    {
        float possible = GameHelper.Random(0f, 1f);
        return possible <= 0.05f;
    }
    #endregion
}
