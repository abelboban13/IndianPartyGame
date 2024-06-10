using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class S_BoardManager : S_Singleton<S_BoardManager>
{
    public S_Space startingSpace;

    [SerializeField] private float _playerSpeed = 5f;
    [SerializeField] private GameObject _player;
    [SerializeField] private S_BoardCameraController _camera;

    public List<S_BoardPlayer> _players;
    private int _playerIndex = 0; //the player whos turn it is

    private PlayerInputManager _playerInputManager;
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

    public int coins { get; private set; }

    public S_GameEvent boardStartEvent;

    [HideInInspector] public bool _joining = false;

    private bool _playerJoinedEvent = false;


    // Start is called before the first frame update
    void Start()
    {
        // StartGame();
        _playerInputManager = GetComponent<PlayerInputManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Ends the current players turn and sets the next one
    public void TurnEnd()
    {
        if(_playerIndex >= _players.Count - 1)
            _playerIndex = 0;
        else
            _playerIndex++;
        Debug.Log($"player {_playerIndex + 1}'s turn");
        _players[_playerIndex].StartTurn();
        _camera.FollowPLayer(_players[_playerIndex]);
    }

    /// <summary>
    /// sets board to starting state
    /// </summary>
    public void StartGame()
    {
        _joining = true;    

        _playerInputManager.EnableJoining();


        StartCoroutine(PlayerJoin());
    }

    public void UnloadBoard()
    {
        foreach(S_BoardPlayer player in _players)
        {
            player.OnUnloadBoard();
        }
    }

    public void LoadBoard()
    {
        foreach (S_BoardPlayer player in _players)
        {
            player.OnReloadBoard();
        }
    }


    public void OnPlayerJoin()
    {
        _playerJoinedEvent = true;
    }
    IEnumerator PlayerJoin()
    {
        int playerNum = 1;
        while (_joining)
        {
            if(playerNum > 4)
                EndJoin();


            Debug.Log($"player {playerNum} press Start(Start on controller, Enter on keyboard) if no more players wish to join press Confirm(A on controller, Space on keyboard)");


            yield return new WaitUntil(() => _playerJoinedEvent == true );
            _playerJoinedEvent = false;
            playerNum++;
        }


        _playerInputManager.DisableJoining();
        yield return null;
    }

    public void EndJoin()
    {
        _joining = false;
        StopCoroutine(PlayerJoin());
        _playerInputManager.DisableJoining();
        boardStartEvent.Raise();
        _players[0].StartTurn();
        _camera.FollowPLayer(_players[0]);
    }


}
