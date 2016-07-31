using UnityEngine;
using System.Collections;

public class HPTrap : InteractableBase {
    

    [SerializeField]
    public float AttackPower = 1f;

    protected override void OnResponse2Player(ImmersiveReceiver player)
    {
        player.HPChange(-AttackPower);
    }
}
