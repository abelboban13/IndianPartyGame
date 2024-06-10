using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class S_RedLightGame : S_MiniGame
{
    [SerializeField] GameObject _playerPrefab;

    [HideInInspector] public List<S_RedLightPlayer> players;

    public S_TrafficLight trafficLight;

    [SerializeField] private List<Transform> _spawns;

    public override void LoadGame()
    {
        base.LoadGame();
        int playerIndex = 0;
        foreach (var player in S_BoardManager.Instance._players) 
        {
            GameObject newPlayer = Instantiate(_playerPrefab);
            newPlayer.transform.position = _spawns[0].position;
            player.GetComponent<S_InputController>().GiveInputData(newPlayer.GetComponent<S_InputController>());
            newPlayer.GetComponent<S_RedLightPlayer>().playerIndex = playerIndex; 
            playerIndex++;
        }
    }

    public void PlayerKnockedOut()
    {
        int playersOut = 0;
        foreach(S_RedLightPlayer player in players)
        {
            if (player.isOut)
                playersOut++;
            
        }
        if (playersOut >= players.Count)
        {
            Debug.Log("all players out");
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

            break;

        case 3:

            break;

        case 4:

            break;

        }

        
    }
}
