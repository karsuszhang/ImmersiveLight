using UnityEngine;
using System.Collections;

public class Enemy : InteractableBase {

    [SerializeField]
    public float AlarmDis = 10f;

    [SerializeField]
    public float Speed = 1f;

    [SerializeField]
    public float AttackPower = 2f;

    void LateUpdate()
    {
        CheckPlayer();
    }

    void CheckPlayer()
    {
        if (Game.Instance.CurPlayer != null)
        {
            Vector3 dis = (Game.Instance.CurPlayer.Pos - this.transform.position);
            if (dis.magnitude <= AlarmDis)
            {
                this.transform.forward = dis.normalized;
                this.transform.position += this.transform.forward * Speed * Time.deltaTime;
            }
        }
    }

    protected override void OnResponse2Player(ImmersiveReceiver player)
    {
        player.HPChange(-AttackPower);
    }
}


