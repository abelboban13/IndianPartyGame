using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class S_GameManager : S_Singleton<S_GameManager>
{

    private int _numberOfPlayers;
    public int numberOfPlayers
    {
        get { return _numberOfPlayers; }
        private set { _numberOfPlayers = value; }
    }

   // public GameObject currentMiniGame;

    public GameMode GameType { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        numberOfPlayers = 4;

        if(GameType == GameMode.Board)
            S_BoardManager.Instance.StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            LoadBoard();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            LoadMiniGame("SC_MiniGameGym");
        }
    }


    public void LoadMiniGame(string minigameName)
    {
        S_BoardManager.Instance.UnloadBoard();
        SceneManager.LoadScene(minigameName);
        GameType = GameMode.Minigame;
    }

    public void LoadBoard()
    {
        S_BoardManager.Instance.LoadBoard();
        SceneManager.LoadScene("SC_BoardGym");
        GameType = GameMode.Board;
    }


    public enum GameMode
    {
        Board,
        Minigame
    }
}
