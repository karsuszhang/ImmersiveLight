using UnityEngine;
using System.Collections;

public class MainUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponentOnChild<UILabel>("ScoreLabel").text = PlayerInfo.Instance.ScoreInBag.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
