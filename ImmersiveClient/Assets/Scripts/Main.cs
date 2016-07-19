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

    public void LoadLevel1()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level2-1");
    }

    public void LoadLevel2()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
    }

    public void LoadLevel3()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level9");
    }
}
