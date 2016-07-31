using UnityEngine;
using System.Collections;

public abstract class InteractableBase : CommonObjBase {

    [SerializeField]
    public float AttackInterval = 1f;

    private bool AttackFrame = false;
    private float m_TimeCount = 0f;


    void FixedUpdate () {
        AttackFrame = false;

        m_TimeCount += Time.fixedDeltaTime;
        if (m_TimeCount >= AttackInterval)
        {
            m_TimeCount -= AttackInterval;
            AttackFrame = true;
        }
    }

	// Use this for initialization
	void Start () {
        _Start();
	}

    protected virtual void _Start()
    {
        GameHelper.SetLayer(gameObject, (int)SpecifiedLayer.InteractableObj);
    }

    public void PlayerCD(ImmersiveReceiver player)
    {
        if (!AttackFrame)
            return;

        OnResponse2Player(player);
    }

    protected virtual void OnResponse2Player(ImmersiveReceiver player)
    {
    }
}
