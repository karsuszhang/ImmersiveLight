using UnityEngine;
using System.Collections;

public enum SpecifiedLayer
{
    PlayerObj = 8,
    MapObj = 9,
    InteractableObj = 10,
}

public enum ColorSlotType
{
    R,
    G,
    B,
    All,
}

public abstract class CommonObjBase :MonoBehaviour
{
}

public static class GameHelper {
    static System.Random s_Randomer;

    public static Vector3 RandomNormalizedVector3(bool zero_y = false)
    {
        Vector3 ret = new Vector3(GameHelper.Random(-1f, 1f), GameHelper.Random(-1f, 1f), GameHelper.Random(-1f, 1f));
        while(ret.magnitude <= Constant.FloatEplison)
            ret = new Vector3(GameHelper.Random(-1f, 1f), GameHelper.Random(-1f, 1f), GameHelper.Random(-1f, 1f));

        if (zero_y)
        {
            ret.Normalize();
            ret.y = 0;
        }
        return ret.normalized;
    }

    //note!! [min, max]
    public static int Random(int min, int max)
    {
        if (s_Randomer == null)
            s_Randomer = new System.Random(System.DateTime.Now.Millisecond);

        return s_Randomer.Next(min, max + 1);
    }

    public static float Random(float min, float max)
    {
        if (s_Randomer == null)
            s_Randomer = new System.Random(System.DateTime.Now.Millisecond);

        int p = Random(0, 100);

        return min + (float)p / 100f * (max - min);
    }

    public static void SetLayer(GameObject obj, int layer)
    {
        obj.layer = layer;
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            SetLayer(obj.transform.GetChild(i).gameObject, layer);
        }
    }

    public static T GetTypeUpAbove<T>(GameObject obj) where T : CommonObjBase
    {
        T ret = obj.GetComponent<T>();
        if (ret != null)
            return ret;

        if (obj.transform.parent == null)
            return null;

        return GetTypeUpAbove<T>(obj.transform.parent.gameObject);
    }

    public static float GetColor(this Color c, ColorSlotType t)
    {
        switch (t)
        {
            case ColorSlotType.R:
                return c.r;
            case ColorSlotType.G:
                return c.g;
            case ColorSlotType.B:
                return c.b;
            case ColorSlotType.All:
                return c.r;
        }

        return 0f;
    }

    public static void SetColor(this Color c, ref Color s_c, ColorSlotType t, float v)
    {
        switch (t)
        {
            case ColorSlotType.R:
                s_c.r = v;
                return;
            case ColorSlotType.G:
                s_c.g = v;
                return;
            case ColorSlotType.B:
                s_c.b = v;
                return;
        }
    }

    public static Color LerpColorByMainColor(Color start, Color dest, float ratio)
    {
        ColorSlotType main_type = FindMainColor(dest);
        if (main_type == ColorSlotType.All)
        {
            main_type = FindMainColor(start);
            if (main_type == ColorSlotType.All)
            {
                return (1f - ratio) * start + ratio * dest;
            }
        }

        Color ret = new Color();
        ret.SetColor(ref ret, main_type, dest.GetColor(main_type));
        for (ColorSlotType i = ColorSlotType.R; i < ColorSlotType.All; i++)
        {
            if (i == main_type)
                continue;

            ret.SetColor(ref ret, i, start.GetColor(i) * (1f - ratio) + dest.GetColor(i) * ratio);
        }

        return ret;
    }

    private static ColorSlotType FindMainColor(Color c)
    {
        if (Equal(c.r, c.g) && Equal(c.g , c.b))
            return ColorSlotType.All;

        ColorSlotType ret;

        if (c.r > c.g)
            ret = ColorSlotType.R;
        else
            ret = ColorSlotType.G;

        if (c.GetColor(ret) < c.b)
            ret = ColorSlotType.B;

        return ret;
    }

    public static bool Equal(float a, float b)
    {
        if (Mathf.Abs(a - b) <= Constant.FloatEplison)
            return true;

        return false;
    }

    public static void DebugBreak(string info = "")
    {
        #if UNITY_EDITOR
        if(!string.IsNullOrEmpty(info))
        {
            Debug.LogError(info);
        }
        Debug.Break();
        #endif
    }

    public static float ToAbsPi(float angle)
    {
        while (angle > 180f)
            angle -= 360f;

        while (angle < -180f)
            angle += 360f;

        return angle;
    }

    public static GameObject FindChild(this GameObject o, string name, bool recursive = false)
    {
        for (int i = 0; i < o.transform.childCount; i++)
        {
            Transform c = o.transform.GetChild(i);
            if (c.gameObject.name == name)
                return c.gameObject;

            if (recursive)
            {
                GameObject ret = c.gameObject.FindChild(name, recursive);
                if (ret != null)
                    return ret;
            }                
        }

        Transform r = o.transform.FindChild(name);
        if (r != null)
            return r.gameObject;

        return null;
    }

    public static T GetComponentOnChild<T>(this GameObject o, string name, bool recursive = false) where T : MonoBehaviour
    {
        GameObject ret = o.FindChild(name, recursive);
        if (ret == null)
            return null;

        return ret.GetComponent<T>();
    }
}
