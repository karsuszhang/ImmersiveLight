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

    public float HP { get ; private set;}

    public const float MAX_HP = 10f;

	//public event OnReceiverDestRatioChanged RatioChangeEvent;

	private float m_BaseIntensity;
	private float m_CurIntensity;
	private MeshRenderer m_Render = null;
	private Light m_Light = null;
    private SphereCollider m_Collider = null;

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

    void FixedUpdate()
    {
        CheckOverlapInteractable();
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
    }

	public override void CheckCD(BaseCDObj c)
	{
		if (c.Type == ObjectType.LightPlus)
		{
			RaycastHit final = FindCollideWithLightPlus(c as LightPlus);

			if (final.collider != null)
			{
				LightPlus lp = (c as LightPlus);
				lp.EndAt(final.point, this);
				//if (lp.LightColor == ReceiveColor)
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

    public void Stop()
    {
        InputMover im = gameObject.GetComponent<InputMover>();
        if (im != null)
        {
            im.Dir = Vector3.zero;
        }
    }
}
