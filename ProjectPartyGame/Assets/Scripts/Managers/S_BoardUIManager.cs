using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_BoardUIManager : S_Singleton<S_BoardUIManager>
{
    [SerializeField] public S_HUD[] playerHuds;
    [SerializeField] private S_EndScreen _endScreen;
    [SerializeField] private S_PauseScreen _pauseScreen;
    private int players = 0;
    private bool paused;
    // Start is called before the first frame update
    void Start()
    {
        _endScreen.gameObject.SetActive(false);
        _pauseScreen.gameObject.SetActive(false);   
        foreach (var player in playerHuds)
        {
            player.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //updates Hud elements coin, mango and trap counters based on number of players
        if(players > 0 && S_GameManager.Instance.GameType == S_GameManager.GameMode.Board)
        {
            for (int i = 0; i < S_BoardManager.Instance._players.Count; i++)
            {
                S_BoardPlayer player = S_BoardManager.Instance._players.Find(x => x.index == i);
                playerHuds[i].coins.text = $"Coins: {player.coins}";
                playerHuds[i].mangos.text = $"Mangos: {player.mangos}";
                playerHuds[i].traps.text = $"Traps: {player.numberOfTraps}";
            }
        }
    }

    public void AddHud()
    {
        playerHuds[players].gameObject.SetActive(true);
        players++;
    }

    public void EndGameScreen(object playerList)
    {
        List<S_BoardPlayer> players = playerList as List<S_BoardPlayer>;  
        _endScreen.gameObject.SetActive(true);
    }

    public void PauseGame(S_InputController player)
    {
        _pauseScreen.gameObject.SetActive(true);
        _pauseScreen.Paused(player);
        Time.timeScale = 0;     
    }

    public void UnPause()
    {
        _pauseScreen.gameObject.SetActive(false);
        Time.timeScale = 1;
    }


}
