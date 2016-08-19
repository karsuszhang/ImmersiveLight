using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //CommonUtil.CommonLogger.ShowLogOnScreen = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("ImmersiveMove");
    }
}
