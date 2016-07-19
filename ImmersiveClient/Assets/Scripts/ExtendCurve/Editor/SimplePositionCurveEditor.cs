using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SimplePositionCurve))]
public class SimplePositionCurveEditor : AnimatedCurveEditor {

    public override void OnInspectorGUI()
    {
        GUILayout.Space(6f);
        NGUIEditorTools.SetLabelWidth(120f);

        SimplePositionCurve tw = target as SimplePositionCurve;
        GUI.changed = false;

        Vector3 from = EditorGUILayout.Vector3Field("From", tw.From);
        Vector3 to = EditorGUILayout.Vector3Field("To", tw.To);

        if (GUI.changed)
        {
            NGUIEditorTools.RegisterUndo("AnimateCurve Change", tw);
            tw.From = from;
            tw.To = to;
            NGUITools.SetDirty(tw);
        }

        DrawCommonProperties();

    }
}
