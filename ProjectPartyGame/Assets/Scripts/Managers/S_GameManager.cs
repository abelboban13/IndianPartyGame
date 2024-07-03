using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;

public class S_GameManager : S_Singleton<S_GameManager>
{

    public S_MiniGame currentMiniGame;

    public GameMode GameType { get; private set; }

    [SerializeField] private string[] _minigames;
    [SerializeField] private int _turnLimit = 10;
    public int turnLimit { get { return _turnLimit; } }

    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
            LoadMiniGame();
        }
    }


    public void LoadMiniGame()
    {
        S_BoardManager.Instance.UnloadBoard();
        SceneManager.LoadScene(_minigames[Random.Range(0,_minigames.Length)]);
        GameType = GameMode.Minigame;
    }

    public void LoadBoard()
    {
        SceneManager.LoadScene("BoardSetup");
        S_SpaceManager.Instance.LoadSpaces();
        S_BoardManager.Instance.LoadBoard();
        GameType = GameMode.Board;
    }


    public void SetGameType(GameMode gameMode)
    {
        GameType = gameMode;
    }

    public void EndGame()
    {
        SceneManager.LoadScene("WinSceen");
    }

    public enum GameMode
    {
        Board,
        Minigame
    }
}
