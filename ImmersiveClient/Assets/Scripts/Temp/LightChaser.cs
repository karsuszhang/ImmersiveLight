using UnityEngine;
using System.Collections;

public class LightChaser : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {

        foreach (BaseCDObj o in Game.Instance.AllObjs)
        {
            if (!o.Released && (o is LightPlus))
            {
                this.transform.LookAt(o.Pos);
                break;
            }
        }

	}
}
