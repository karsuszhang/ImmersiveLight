using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PositionChangeCurve))]
public class PositionChangeCurveEditor : AnimatedCurveEditor {

    public override void OnInspectorGUI()
    {
        GUILayout.Space(6f);
        NGUIEditorTools.SetLabelWidth(120f);

        PositionChangeCurve tw = target as PositionChangeCurve;
        GUI.changed = false;

        Vector3 from = EditorGUILayout.Vector3Field("From", tw.From);
        Vector3 to = EditorGUILayout.Vector3Field("Change", tw.Change);

        if (GUI.changed)
        {
            NGUIEditorTools.RegisterUndo("AnimateCurve Change", tw);
            tw.From = from;
            tw.Change = to;
            NGUITools.SetDirty(tw);
        }

        DrawCommonProperties();

    }
}
