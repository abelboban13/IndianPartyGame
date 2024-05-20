using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_BoardManager : S_Singleton<S_BoardManager>
{
    public S_Space startingSpace;

    [SerializeField] private float _playerSpeed = 5f;
    [SerializeField] private GameObject _player;

    public List<S_BoardPlayer> _players;
    private int _playerIndex = 0; // the player whos turn it is
    public float playerSpeed 
    { 
        get
        {
            return _playerSpeed;
        }
        private set
        {
            _playerSpeed = value;
        }
    }

    public S_GameEvent boardStartEvent;

    // Start is called before the first frame update
    void Start()
    {
       // StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnEnd()
    {
        if(_playerIndex >= _players.Count - 1)
            _playerIndex = 0;
        else
            _playerIndex++;
        _players[_playerIndex].StartTurn();    
    }

    /// <summary>
    /// sets board to starting state
    /// </summary>
    public void StartGame()
    {
        for(int i = 0; i < S_GameManager.Instance.numberOfPlayers; i++)
        {
            Instantiate(_player);
        }
        boardStartEvent.Raise();
        _players[0].StartTurn();
    }

    public void LoadBoard()
    {
        //will load the board at its previous state
    }

}
