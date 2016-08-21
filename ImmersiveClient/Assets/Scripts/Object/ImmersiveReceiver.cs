using UnityEngine;
using System.Collections;

public class ImmersiveReceiver : BaseCDObj {

	[SerializeField]
	public float DestIntensity = 3;

	[SerializeField]
	public Color ReceiveColor = Color.white;

	[SerializeField]
	public float AbsorbRate = 0.2f;

	[SerializeField]
	public float DropRate = 0.001f;

    [SerializeField]
    public float Speed = 2f;

    public Vector3 Dir = Vector3.zero;

    public float HP { get ; private set;}

    public const float MAX_HP = 10f;

	//public event OnReceiverDestRatioChanged RatioChangeEvent;

	private float m_BaseIntensity;
	private float m_CurIntensity;
	private MeshRenderer m_Render = null;
	private Light m_Light = null;
    private SphereCollider m_Collider = null;

    private int m_FingerID = -1;
    private Vector3 m_LastPos;

	public ImmersiveReceiver() : base(ObjectType.Receiver)
	{
        HP = MAX_HP;
	}

    protected override void _Awake()
    {
        base._Awake();
        Game.Instance.RegCurPlayer(this);
    }
	protected override void _Start()
	{
		base._Start();
		m_BaseIntensity = gameObject.GetComponentInChildren<Light>().intensity;
		m_CurIntensity = m_BaseIntensity;
		gameObject.GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", ReceiveColor);
		gameObject.GetComponentInChildren<Light>().color = ReceiveColor;
		m_Render = gameObject.GetComponentInChildren<MeshRenderer>();
		m_Light = gameObject.GetComponentInChildren<Light>();
        m_Collider = gameObject.GetComponentInChildren<SphereCollider>();

        gameObject.GetComponentInChildren<Light>().intensity = DestIntensity;
	}
	// Update is called once per frame
	void Update () {
		/*if (m_CurIntensity > m_BaseIntensity)
		{
			m_CurIntensity = Mathf.Max(m_BaseIntensity, m_CurIntensity - DropRate);
			SetRatio((m_CurIntensity - m_BaseIntensity) / DestIntensity);
			m_Light.intensity = m_CurIntensity;
		}*/
	}

    void LateUpdate()
    {
        #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            Dir = new Vector3(0f, 0f, -1f);
        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            Dir = new Vector3(0f, 0f, 1f);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            Dir = new Vector3(1f, 0f, 0f);
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Dir = new Vector3(-1f, 0f, 0f);
        }
        #else
        #endif

        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began)
            {
                //CommonUtil.Logger.Log("a touch begin " + t.fingerId + " cur record " + m_FingerID);
                if (m_FingerID == -1)
                {
                    m_FingerID = t.fingerId;
                    m_LastPos = new Vector3(t.position.x, 0f, t.position.y);
                    CommonUtil.Logger.Log("Begin touch " + t.fingerId);
                }

            }
            else if (t.phase == TouchPhase.Moved && m_FingerID == t.fingerId)
            {
                Vector3 n = new Vector3(t.position.x, 0f, t.position.y);
                Dir = (n - m_LastPos).normalized;
                //m_LastPos = n;
                //CommonUtil.Logger.Log("Dir " + Dir.ToString());
            }
            else if ((t.phase == TouchPhase.Canceled || t.phase == TouchPhase.Ended) && m_FingerID == t.fingerId)
            {
                m_FingerID = -1;
                CommonUtil.Logger.Log("Release Touch");
            }
        }

        CheckMove();
    }

    void FixedUpdate()
    {
        CheckObj();
        CheckOverlapInteractable();
    }

    void CheckMove()
    {
        int layer_mask = 1 << (int)SpecifiedLayer.MapObj;
        Vector3 nextpos = this.Pos + Time.deltaTime * Speed * Dir;
        Collider[] cds = Physics.OverlapSphere(nextpos, m_Collider.radius, layer_mask);
        if (cds == null || cds.Length == 0)
        {
            this.Pos = nextpos;
        }
    }

    void CheckObj()
    {
        int layer_mask = 1 << (int)SpecifiedLayer.PlayerObj;
        Collider[] cds = Physics.OverlapSphere(this.transform.position, m_Collider.radius, layer_mask);
        foreach (Collider cd in cds)
        {
            LightPlus lp = GameHelper.GetTypeUpAbove<LightPlus>(cd.gameObject);
            if (lp != null)
            {
                lp.EndAt(lp.Pos, this);
                {
                    m_CurIntensity += lp.LightIntensity * AbsorbRate;
                    //CommonUtil.CommonLogger.Log(string.Format("{0} Cur {1} Dest {2}", gameObject.name, m_CurIntensity, DestIntensity));
                    if (m_CurIntensity >= DestIntensity)
                    {
                        m_CurIntensity -= DestIntensity;
                        m_CurIntensity = Mathf.Max(m_CurIntensity, m_BaseIntensity);
                        Game.Instance.AddScore(1);
                    }

                    float ratio = Mathf.Min(1f, (m_CurIntensity - m_BaseIntensity) / (DestIntensity - m_BaseIntensity));

                    //gameObject.GetComponentInChildren<Light>().intensity = m_CurIntensity;
                    SetRatio(ratio);
                }
            }
        }
    }

    void CheckOverlapInteractable()
    {
        int layer_mask = 1 << (int)SpecifiedLayer.InteractableObj;
        Collider[] cds = Physics.OverlapSphere(this.transform.position, m_Collider.radius, layer_mask);
        foreach (Collider cd in cds)
        {
            InteractableBase hpt = GameHelper.GetTypeUpAbove<InteractableBase>(cd.gameObject);
            if (hpt != null)
            {
                hpt.PlayerCD(this);
            }
        }
    }

	private void SetRatio(float r)
	{

		gameObject.GetComponentInChildren<MeshRenderer>().material.SetFloat("_Threshold", r);
		//if (RatioChangeEvent != null)
		//	RatioChangeEvent(r, this);
	}

    public void HPChange(float change)
    {
        HP += change;
        HP = Mathf.Min(HP, MAX_HP);

        float ratio = HP / MAX_HP;
        float intensity = m_BaseIntensity + (DestIntensity - m_BaseIntensity) * ratio;

        gameObject.GetComponentInChildren<Light>().intensity = intensity;

        if (HP <= 0)
            Game.Instance.GameOver();
    }
        

    public void Stop()
    {
        this.Dir = Vector3.zero;
    }
}
