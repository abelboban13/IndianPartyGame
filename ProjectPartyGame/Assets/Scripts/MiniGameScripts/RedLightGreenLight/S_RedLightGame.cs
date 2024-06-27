using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class S_RedLightGame : S_MiniGame
{
    [SerializeField] GameObject _playerPrefab;

    [HideInInspector] public List<S_RedLightPlayer> players;

    public S_TrafficLight trafficLight;

    [SerializeField] private List<Transform> _spawns;

    [SerializeField] private TextMeshProUGUI _timer;

    // var order list

    private int _playersOut;
    private bool _allOut = false;

    private void FixedUpdate()
    {
        _timer.text = Mathf.RoundToInt(trafficLight._gameTimer).ToString();
    }

    public override void LoadGame()
    {
        base.LoadGame();
        int playerIndex = 0;

        for (int i = 0; i < S_BoardManager.Instance._players.Count; i++)
        {
           S_InputController input = S_BoardManager.Instance._players.Find(x => x.index == playerIndex).GetComponent<S_InputController>();
           GameObject newPlayer = Instantiate(_playerPrefab);
           newPlayer.transform.position = _spawns[playerIndex].position;
           input.GiveInputData(newPlayer.GetComponent<S_InputController>());
           newPlayer.GetComponent<S_RedLightPlayer>().playerIndex = playerIndex;
           playerIndex++;
        }
        SetUpCamera();
        gameRunning = true;
    }

    public void PlayerKnockedOut()
    {
        int playersOut = 0;
        foreach(S_RedLightPlayer player in players)
        {
            if(player.isOut)
                playersOut++;
        }
        if (playersOut >= players.Count)
        {
            _allOut = true;
            gameRunning = false;
            EndGame();
        }
    }

    public void SetUpCamera()
    {
        switch (players.Count)
        {
        case 1:

            break;

        case 2:
            players[0].playerCamera.rect = new Rect(0, 0, .5f,1);
            players[1].playerCamera.rect = new Rect(.5f, 0, .5f, 1);
            break;

        case 3:
                players[0].playerCamera.rect = new Rect(0, .5f, .5f, .5f);
                players[1].playerCamera.rect = new Rect(.5f, 0, .5f, .5f);
                players[2].playerCamera.rect = new Rect(0, 0, .5f, .5f);
                break;

        case 4:
                players[0].playerCamera.rect = new Rect(0, .5f, .5f, .5f);
                players[1].playerCamera.rect = new Rect(.5f, 0, .5f, .5f);
                players[2].playerCamera.rect = new Rect(0, 0, .5f, .5f);
                players[2].playerCamera.rect = new Rect(.5f, 0, .5f, .5f);
                break;
        }  
    }

    public override void EndGame()
    {
        gameRunning = false;
        S_BoardManager.Instance.ChangeTurnOrder(trafficLight.CreatePodium(players, _allOut));
        base.EndGame();
    }
}
