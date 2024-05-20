using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_GameManager : S_Singleton<S_GameManager>
{

    private int _numberOfPlayers;
    public int numberOfPlayers
    {
        get { return _numberOfPlayers; }
        private set { _numberOfPlayers = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        numberOfPlayers = 4;
        S_BoardManager.Instance.StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public enum GameMode
    {
        Board,
        Minigame
    }
}
