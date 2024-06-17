using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor.SearchService;
using UnityEditor;

public class S_GameManager : S_Singleton<S_GameManager>
{

    public S_MiniGame currentMiniGame;

    public GameMode GameType { get; private set; }

    [SerializeField] private List<SceneAsset> _minigames;
    // Start is called before the first frame update
    void Start()
    {

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
        SceneManager.LoadScene(_minigames[Random.Range(0,_minigames.Count)].name);
        GameType = GameMode.Minigame;
    }

    public void LoadBoard()
    {
        S_BoardManager.Instance.LoadBoard();
        SceneManager.LoadScene("SC_BoardGym");
        GameType = GameMode.Board;
    }


    public void SetGameType(GameMode gameMode)
    {
        GameType = gameMode;
    }

    public enum GameMode
    {
        Board,
        Minigame
    }
}
