using UnityEngine;
using System.Collections;

public class InGameMainUI : MonoBehaviour {

    UILabel m_ScoreLabel = null;
	// Use this for initialization
	void Start () {
        m_ScoreLabel = this.transform.FindChild("ScoreLabel").gameObject.GetComponent<UILabel>();
        m_ScoreLabel.text = "0";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetScore(int score)
    {
        m_ScoreLabel.text = score.ToString();
    }
}
