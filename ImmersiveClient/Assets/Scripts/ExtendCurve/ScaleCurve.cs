using UnityEngine;
using System.Collections;

public class ScaleCurve : AnimatedCurve {

    public Vector3 From = Vector3.one;
    public Vector3 To = Vector3.one;

    protected override void OnUpdate(float factor, bool isFinished)
    {
        this.transform.localScale = From * (1f - factor) + factor * To;
    }
}
