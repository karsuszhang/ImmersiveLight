using UnityEngine;
using System.Collections;

public class HPTrap : CommonObjBase {
    [SerializeField]
    public float AttackInterval = 1f;

    [SerializeField]
    public float AttackPower = 1f;

    private bool AttackFrame = false;
    private float m_TimeCount = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	
	void FixedUpdate () {
        AttackFrame = false;

        m_TimeCount += Time.fixedDeltaTime;
        if (m_TimeCount >= AttackInterval)
        {
            m_TimeCount -= AttackInterval;
            AttackFrame = true;
        }
	}

    public void PlayerCD(ImmersiveReceiver player)
    {
        if (!AttackFrame)
            return;

        player.HPChange(-AttackPower);
    }
}
