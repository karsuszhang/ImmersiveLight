using UnityEngine;
using System.Collections;

public class SingleReceiverInfluenceObj : ReceiverInfluenceObj {

    [SerializeField]
    public Receiver TheReceiver;
	// Use this for initialization
	void Start () {
        if (TheReceiver != null)
            TheReceiver.RatioChangeEvent += this.OnRatioChanged;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void OnRatioChanged(float ratio, Receiver obj)
    {
        gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Threshold", ratio);
    } 
}
