using UnityEngine;
using System.Collections;

public delegate void OnStablizerHoldPlayer(bool is_hold);
public class Stablizer : MonoBehaviour {

    [SerializeField]
    public float StabDis = 1f;

    public event OnStablizerHoldPlayer EventHoldPlayer = null;

    bool m_ReceiverIn = false;
	// Use this for initialization
	void Start () {
	
	}

    float Dis2Player()
    {
        Vector3 player_plane = new Vector3(Game.Instance.CurPlayer.Pos.x, 0f, Game.Instance.CurPlayer.Pos.z);
        Vector3 pos_plane = new Vector3(this.transform.position.x, 0f, this.transform.position.z);

        return (player_plane - pos_plane).magnitude;
    }
	
	// Update is called once per frame
	void Update () {
        if (Game.Instance.CurPlayer == null)
            return;
        
        if (!m_ReceiverIn)
        {
            if( Dis2Player() <= StabDis)
            {
                m_ReceiverIn = true;
                Game.Instance.CurPlayer.Stop();
                if (EventHoldPlayer != null)
                    EventHoldPlayer(true);
            }
        }

        if (m_ReceiverIn)
        {
            if (Dis2Player() > StabDis)
            {
                m_ReceiverIn = false;
                if (EventHoldPlayer != null)
                    EventHoldPlayer(false);
            }
        }
	}

    void OnDrawGizmosSelected()
    {
        #if UNITY_EDITOR
        Gizmos.color = Color.blue;
        //Gizmos.DrawSphere(gameObject.transform.position, 0.2f);
        Gizmos.DrawWireSphere(gameObject.transform.position, this.StabDis);
        #endif
    }
}
