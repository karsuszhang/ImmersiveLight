using UnityEngine;
using System.Collections;

public class RotationCurve : AnimatedCurve 
{
    public Vector3 From = Vector3.zero;
    public Vector3 To = Vector3.zero;
    
    protected override void OnUpdate(float factor, bool isFinished)
    {
        this.transform.eulerAngles = From * (1f - factor) + factor * To;
    }
	
}
