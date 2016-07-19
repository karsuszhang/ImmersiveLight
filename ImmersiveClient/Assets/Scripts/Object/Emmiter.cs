using UnityEngine;
using System.Collections;

public class Emmiter : BaseCDObj {

    public Color LightColor = Color.white;

    [SerializeField]
    public float MaxIntensityTime = 3f;

    [SerializeField]
    public float MaxLightSpeed = 10f;

    [SerializeField]
    public float MinLightSpeed = 2f;

    [SerializeField]
    public float MaxLightIntensity = 2f;

    [SerializeField]
    public bool ManualAble = false;

    [SerializeField]
    public float MaxRotateAngle = 60f;

    private float m_TimeCount = 0f;
    private Color m_BaseColor;
    public Emmiter()
        : base(ObjectType.Emmiter)
    {
        
    }

	private int m_PressedFingerID = -1;

	private bool m_IsPressing = false;
	
    private Vector3 m_PressPos = Vector3.zero;
    private Vector3 m_PressDelta = Vector3.zero;
    private Vector3 m_OrgEuler = Vector3.zero;
    protected override void _Start()
    {
        base._Start();
        m_BaseColor = this.gameObject.GetComponentInChildren<MeshRenderer>().material.GetColor("_Color");
        m_OrgEuler = gameObject.transform.eulerAngles;
    }
	// Update is called once per frame
	void Update () {
        if (m_IsPressing)
        {
            m_TimeCount += Time.deltaTime;
            if (m_TimeCount >= MaxIntensityTime)
                m_TimeCount = MaxIntensityTime;

            Color c = HSBColor.LerpWithMinRatio(m_BaseColor, LightColor, m_TimeCount / MaxIntensityTime);
            gameObject.GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", c);
        }
        #if UNITY_EDITOR
		CheckKeyBoard ();
        CheckMouse();
        #else
        CheckTouch ();
        #endif

        CheckRotate();
	}

    void CheckMouse()
    {
        if (m_IsPressing)
        {
            m_PressDelta = Input.mousePosition - m_PressPos;
            m_PressPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonDown(0))
        {
            m_IsPressing = true;
            m_PressPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
            ReleaseLight ();
    }

	void CheckKeyBoard()
	{
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			m_IsPressing = true;
		}

		if (Input.GetKeyUp (KeyCode.Space))
			ReleaseLight ();
	}

	void CheckTouch()
	{
		foreach (Touch t in Input.touches) 
		{
            if (t.phase == TouchPhase.Began)
            {
                if (m_PressedFingerID == -1)
                {
                    m_PressedFingerID = t.fingerId;
                    m_IsPressing = true;
                    m_PressPos = t.position;
                    CommonUtil.CommonLogger.Log(m_PressPos.ToString());
                    break;
                }
            }
            else if (t.phase == TouchPhase.Canceled || t.phase == TouchPhase.Ended)
            {
                if (m_PressedFingerID == t.fingerId && m_IsPressing)
                {
                    m_PressedFingerID = -1;
                    ReleaseLight();
                    break;
                }
            }
            else if (t.phase == TouchPhase.Moved)
            {
                if (m_PressedFingerID == t.fingerId)
                {
                    m_PressDelta = (Vector3)t.position - m_PressPos;
                    m_PressPos = t.position;
                }
            }
		}
	}

    void CheckRotate()
    {
        if (!ManualAble || !m_IsPressing)
            return;

        Vector3 ea = this.transform.eulerAngles;
        ea.y = GameHelper.ToAbsPi(ea.y);

        ea.y = GameHelper.ToAbsPi(ea.y - m_PressDelta.x);

        float l_bound = Mathf.Min(GameHelper.ToAbsPi(m_OrgEuler.y - MaxRotateAngle), GameHelper.ToAbsPi(m_OrgEuler.y + MaxRotateAngle));
        float u_bound = Mathf.Max(GameHelper.ToAbsPi(m_OrgEuler.y - MaxRotateAngle), GameHelper.ToAbsPi(m_OrgEuler.y + MaxRotateAngle));
         
        ea.y = Mathf.Max(l_bound, Mathf.Min(u_bound, ea.y));
        this.transform.eulerAngles = ea;
        //CommonUtil.CommonLogger.Log("Adjust euler " + ea.y.ToString() + " after " + this.transform.eulerAngles);
        m_PressDelta = Vector3.zero;
    }

	void ReleaseLight()
	{
        float ratio = m_TimeCount / MaxIntensityTime;
        ReleaseLight(ratio);
	}

    public void ReleaseLight(float ratio)
    {
        Color c = LightColor;//HSBColor.LerpWithMinRatio(m_BaseColor, LightColor, ratio);

        LightPlus lo = LightPlus.GenLightPlus();
        lo.Dir = this.Dir;
        lo.Pos = this.Pos;
        lo.Speed = MinLightSpeed + ratio * (MaxLightSpeed - MinLightSpeed);
        lo.SetColor(c, ratio * MaxLightIntensity);
        lo.SetScaleRatio(ratio, true);
        lo.LightEmmiter = this;
        m_IsPressing = false;

        m_TimeCount = 0f;
        gameObject.GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", m_BaseColor);
    }

    public void SetColorLerp(float ratio)
    {
        Color c = HSBColor.LerpWithMinRatio(m_BaseColor, LightColor, ratio);
        gameObject.GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", c);
    }

    public void SetBaseColor(Color c)
    {
        LightColor = c;
    }
}
