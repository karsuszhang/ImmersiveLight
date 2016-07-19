using UnityEngine;
using System.Collections;

public class InputMover : MonoBehaviour 
{
    [SerializeField]
    public float Speed = 1f;

    private Vector3 Dir = Vector3.zero;
	// Use this for initialization
	void Start () {
        GameObject.FindObjectOfType<FollowCamera>().RegFollowObj(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () 
    {
        this.transform.position += Time.deltaTime * Speed * Dir;

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
}
