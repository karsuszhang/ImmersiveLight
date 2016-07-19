using UnityEngine;
using System.Collections;

public enum ObjectType
{
    None,
    LightPlus,
    Emmiter,
    DistinguishBlock,
    Block,
    Reflector,
    Receiver,
    EmmiterColorChanger,
}

public class BaseCDObj : MonoBehaviour {

    protected BaseCDObj(ObjectType t)
    {
        Type = t;
        Released = false;
    }

    public ObjectType Type{ get; private set; }
    public bool Released { get; protected set; }
    public Vector3 Dir
    {
        get
        {
            return this.gameObject.transform.forward;
        }
        set
        {
            this.gameObject.transform.forward = value;
        }
    }

    public Vector3 Pos
    {
        get
        {
            return this.gameObject.transform.position;
        }
        set
        {
            LastPos = Pos;
            this.gameObject.transform.position = value;
        }
    }

    public Vector3 LastPos{ get; private set; }

    void Awake()
    {
        _Awake();
    }

	// Use this for initialization
	void Start () {
        _Start();
        LastPos = Pos;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy()
    {
        Game.Instance.UnRegObject(this);
    }

    protected virtual void _Start()
    {
        Game.Instance.RegCDObj(this);
    }

    protected virtual void _Awake()
    {
    }

    public virtual void CheckCD(BaseCDObj c)
    {

    }

    public virtual void Release()
    {
        /*if(gameObject.name.Contains("sat"))
        {
            GameHelper.DebugBreak(gameObject.name);
        }*/
        Released = true;
        CommonUtil.ResourceMng.Instance.Release(this.gameObject);
    }

    protected RaycastHit FindCollideWithLightPlus(LightPlus lp)
    {
        Collider[] cds = gameObject.GetComponentsInChildren<Collider>();
        RaycastHit final = new RaycastHit();
        Ray r = new Ray();
        r.origin = lp.Pos;
        r.direction = lp.Dir;
        FindNearestCD(r, cds, lp.RadiusLength, out final);
        if (final.collider == null)
        {
            FindNearestCD(lp.LastPos, lp.Pos, cds, out final);
            if (final.collider != null)
            {
                //CommonUtil.CommonLogger.Log(gameObject.name + " Reflect LightPlus " + lp.gameObject.name + " by two point");
                //EditorGizmor.Instance.DrawLine(lp.LastPos, lp.Pos);
            }
        }
        else
        {
            //CommonUtil.CommonLogger.Log(gameObject.name + " Reflect LightPlus " + lp.gameObject.name + " by normal cd");
        }
        return final;
    }

    public static void FindNearestCD(Ray r, Collider[] cds, float lenght, out RaycastHit final_info)
    {
        final_info = new RaycastHit();
        float len = 999999f;
        foreach(Collider cd in cds)
        {
            RaycastHit info;
            if (cd.Raycast(r, out info, lenght))
            {
                if (info.distance < len)
                    final_info = info;
            }
        }
    }

    public static void FindNearestCD(Vector3 start, Vector3 end, Collider[] cds, out RaycastHit final_info)
    {
        Ray r = new Ray();
        r.origin = start;
        r.direction = (end - start).normalized;
        final_info = new RaycastHit();
        float len = 999999f;
        foreach(Collider cd in cds)
        {
            RaycastHit info;
            if (cd.Raycast(r, out info, (end - start).magnitude))
            {
                if (info.distance < len)
                    final_info = info;
            }
        }
    }
}
