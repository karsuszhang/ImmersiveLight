using UnityEngine;
using System.Collections;

public class PlayerInfo
{
    public static PlayerInfo Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlayerInfo();
            }

            return _instance;
        }
    }

    static PlayerInfo _instance = null;


    #region Attr
    public int ScoreInBag
    {
        get
        {
            return _ScoreInBag;
        }
        set
        {
            _ScoreInBag = value;
            PlayerPrefs.SetInt( KeyScoreInBag, _ScoreInBag);
        }
    }
    private int _ScoreInBag = 0;

    #endregion


    #region AttrKey
    const string KeyScoreInBag = "ScoreInBag";

    #endregion
    private PlayerInfo()
    {
        _ScoreInBag = PlayerPrefs.GetInt(KeyScoreInBag);
    }
}
