using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ShoreAndPondGame : S_MiniGame
{
    [SerializeField] private List<Transform> _spawns;
    [HideInInspector] public List<S_ShoreAndPondPlayer> players;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private S_Judge _judge;
    public GameObject[] pondSpots = new GameObject[4];
    [HideInInspector] public List<S_ShoreAndPondPlayer> outPlayers = new List<S_ShoreAndPondPlayer>();



    private bool _allOut = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //loads the players
    public override void LoadGame()
    {
        base.LoadGame();
        int playerIndex = 0;

        for (int i = 0; i < S_BoardManager.Instance._players.Count; i++)
        {
            S_InputController input = S_BoardManager.Instance._players.Find(x => x.index == playerIndex).GetComponent<S_InputController>();
            Debug.Log("player found");
            GameObject newPlayer = Instantiate(_playerPrefab);
            newPlayer.transform.position = _spawns[playerIndex].position;
            input.GiveInputData(newPlayer.GetComponent<S_InputController>());
            newPlayer.GetComponent<S_ShoreAndPondPlayer>().playerIndex = playerIndex;
            playerIndex++;
        }
        gameRunning = true;
    }

    public void PlayerKnockedOut()
    {
        int playersOut = 0;
        foreach (S_ShoreAndPondPlayer player in players)
        {
            if (player.isOut)
                playersOut++;
        }
        if (playersOut >= players.Count - 1)
        {
            gameRunning = false;
            EndGame();
        }
    }
    public override void EndGame()
    {
        gameRunning = false;
        S_BoardManager.Instance.ChangeTurnOrder(CreatePodium());
        base.EndGame();
    }

    private List<int> CreatePodium()
    {
        List<int> results = new List<int>();
        S_ShoreAndPondPlayer winner = players.Find(player => !player.isOut);
        results?.Add(winner.playerIndex);
        outPlayers.Reverse();
        foreach(S_ShoreAndPondPlayer player in outPlayers)
        {
            results.Add(player.playerIndex);
        }
        return results;
    }
}