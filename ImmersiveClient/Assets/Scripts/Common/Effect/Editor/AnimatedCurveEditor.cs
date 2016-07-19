using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(AnimatedCurve), true)]
public class AnimatedCurveEditor : Editor 
{
	public override void OnInspectorGUI ()
	{
		GUILayout.Space(6f);
		NGUIEditorTools.SetLabelWidth(110f);
		base.OnInspectorGUI();
		DrawCommonProperties();
	}

	protected void DrawCommonProperties ()
	{
		AnimatedCurve tw = target as AnimatedCurve;

		if (NGUIEditorTools.DrawHeader("Animation"))
		{
			NGUIEditorTools.BeginContents();
			NGUIEditorTools.SetLabelWidth(110f);

			GUI.changed = false;

			AnimatedCurve.Style style = (AnimatedCurve.Style)EditorGUILayout.EnumPopup("Play Style", tw.style);
			AnimationCurve curve = EditorGUILayout.CurveField("Animation Curve", tw.animationCurve, GUILayout.Width(170f), GUILayout.Height(62f));
			AnimatedCurve.Method method = (AnimatedCurve.Method)EditorGUILayout.EnumPopup("Play Method", tw.method);

			GUILayout.BeginHorizontal();
			float dur = EditorGUILayout.FloatField("Duration", tw.duration, GUILayout.Width(170f));
			GUILayout.Label("seconds");
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			float del = EditorGUILayout.FloatField("Start Delay", tw.delay, GUILayout.Width(170f));
			GUILayout.Label("seconds");
			GUILayout.EndHorizontal();

			bool ts = EditorGUILayout.Toggle("Ignore TimeScale", tw.ignoreTimeScale);

			if (GUI.changed)
			{
				NGUIEditorTools.RegisterUndo("AnimateCurve Change", tw);
				tw.animationCurve = curve;
				tw.method = method;
				tw.style = style;
				tw.ignoreTimeScale = ts;
				tw.duration = dur;
				tw.delay = del;
				NGUITools.SetDirty(tw);
			}
			NGUIEditorTools.EndContents();
		}
			
	}

}
