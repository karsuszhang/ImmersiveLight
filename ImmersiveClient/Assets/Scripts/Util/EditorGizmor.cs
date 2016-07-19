using UnityEngine;
using System.Collections;
using System.Collections.Generic;

struct GLine
{
    public Vector3 start;
    public Vector3 end;
}

public class EditorGizmor : MonoBehaviour {
    public static EditorGizmor Instance
    {
        get
        {
            if (_Instance == null)
            {
                GameObject gizmo = new GameObject();
                _Instance = gizmo.AddComponent<EditorGizmor>();
                GameObject.DontDestroyOnLoad(gizmo);
                gizmo.name = "EditorGizmoHelper";
            }

            return _Instance;
        }
    }

    private static EditorGizmor _Instance = null;

    private List<GLine> m_DrawLines = new List<GLine>();

    [SerializeField]
    public Color DrawColor = Color.yellow;

    public void DrawLine(Vector3 start, Vector3 end)
    {
        GLine l = new GLine();
        l.start = start;
        l.end = end;
        m_DrawLines.Add(l);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        Gizmos.color = DrawColor;

        foreach(var obj in m_DrawLines)
        {
            Gizmos.DrawLine(obj.start, obj.end);
        }
        #endif
    }
}
