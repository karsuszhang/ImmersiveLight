using UnityEngine;
using System.Collections;

public class PositionChangeCurve : AnimatedCurve {

    public Vector3 From = Vector3.zero;
    public Vector3 Change = Vector3.zero;

    protected override void OnUpdate(float factor, bool isFinished)
    {
        this.transform.localPosition = From + factor * Change;
    }
}
