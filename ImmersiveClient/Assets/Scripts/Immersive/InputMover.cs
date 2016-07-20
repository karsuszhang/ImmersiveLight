using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputMover : MonoBehaviour 
{
    [SerializeField]
    public float Speed = 1f;

    private Vector3 Dir = Vector3.zero;

	private float m_CollideR = 0f;
	// Use this for initialization
	void Start () {
        GameObject.FindObjectOfType<FollowCamera>().RegFollowObj(this.gameObject);
		m_CollideR = this.GetComponentInChildren<SphereCollider> ().radius;
	}
	
	// Update is called once per frame
	void LateUpdate () 
    {
		MoveByDir ();

        #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            Dir = new Vector3(0f, 0f, -1f);
        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            Dir = new Vector3(0f, 0f, 1f);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            Dir = new Vector3(1f, 0f, 0f);
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Dir = new Vector3(-1f, 0f, 0f);
        }
        #else
        #endif
	}

	void MoveByDir()
	{
		if (Speed <= Constant.FloatEplison || Dir.magnitude <= Constant.FloatEplison)
			return;
		
		Vector3 next_pos = this.transform.position + Time.deltaTime * Speed * Dir;
		next_pos += m_CollideR * Dir;

		List<BaseCDObj> objs = Game.Instance.AllObjs;
		foreach (BaseCDObj o in objs) 
		{
			if (o.gameObject == this.gameObject)
				continue;

			if ((o.transform.position - this.transform.position).magnitude > 10f * Speed)
				continue;

			if (o.Type == ObjectType.LightPlus)
				continue;

			Collider[] cos = o.gameObject.GetComponentsInChildren<Collider>();
			RaycastHit info;
			BaseCDObj.FindNearestCD(this.transform.position, next_pos, cos, out info);
			if (info.collider != null)
				return;
		}

		this.transform.position += Time.deltaTime * Speed * Dir;
	}
}
