using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class S_BoardUIManager : S_Singleton<S_BoardUIManager>
{
    [SerializeField] public S_HUD[] playerHuds;
    [SerializeField] private S_EndScreen _endScreen;
    [SerializeField] private S_PauseScreen _pauseScreen;
    [SerializeField] private PlayerInventory _playerInventory;
    private EventSystem _eventSystem;
    private int players = 0;
    [HideInInspector] public bool paused;
    // Start is called before the first frame update
    void Start()
    {
        _eventSystem = EventSystem.current;
        _endScreen.gameObject.SetActive(false);
        _pauseScreen.gameObject.SetActive(false);   
        _playerInventory.gameObject.SetActive(false);
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
        if (!S_BoardManager.Instance._joining)
        {
            paused = true;
            _pauseScreen.controller = player;
            _pauseScreen.gameObject.SetActive(true);
            if(S_GameManager.Instance.GameType == S_GameManager.GameMode.Board)
                S_BoardManager.Instance.PausePlayers(player.GetComponent<S_BoardPlayer>());
            
            Time.timeScale = 0;
            _playerInventory.gameObject.SetActive(false);
        }    
    }

    public void UnPause()
    {
        paused= false;
        _pauseScreen.gameObject.SetActive(false);
        Time.timeScale = 1;
        S_BoardManager.Instance.UnPausePlayers();
    }

    public void OpenInventory(S_BoardPlayer boardPlayer)
    {
        if(!paused)
        {
            _playerInventory.player = boardPlayer;
            if (_playerInventory.gameObject.activeSelf)
            {
                _playerInventory.gameObject.SetActive(false);
                S_BoardManager.Instance.UnPausePlayers();
            }
 
            else
            {
                _playerInventory.gameObject.SetActive(true);
                S_BoardManager.Instance.PausePlayers(boardPlayer.GetComponent<S_BoardPlayer>());
            }
                
        }
    }

    public void InputSetUp(GameObject button,S_BoardPlayer boardPlayer)
    {
        _eventSystem.GetComponent<InputSystemUIInputModule>().actionsAsset = boardPlayer.GetComponent<PlayerInput>().actions;
        StartCoroutine(WaitAFrame(button));
    }

    public IEnumerator WaitAFrame(GameObject button)
    {
        _eventSystem.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        _eventSystem.SetSelectedGameObject(button);
        yield return null;
    }
}
