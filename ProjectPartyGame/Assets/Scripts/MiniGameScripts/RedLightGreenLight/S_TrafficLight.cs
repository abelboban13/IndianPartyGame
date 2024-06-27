using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class S_TrafficLight : MonoBehaviour
{

    [SerializeField] private Material _redMaterial;
    [SerializeField] private Material _yellowMaterial;
    [SerializeField] private Material _greenMaterial;
    [SerializeField] private  S_RedLightGame _miniGame;
    [SerializeField] private float _maxTime = 20;

    private int _state = 0;
    private MeshRenderer _renderer;
    public float _gameTimer { get; private set; }
    private float _switchTimer = 0;
    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _miniGame = S_GameManager.Instance.currentMiniGame.GetComponent<S_RedLightGame>();
        _gameTimer = _maxTime;
        SwitchColor();

    }

    // Update is called once per frame
    void Update()
    {
        if (_miniGame.gameRunning && !_miniGame.startPhase)
        {
            if (_state == 3)
            {
                foreach (var player in _miniGame.players)
                {
                    if (player.IsMoving())
                    {
                        Debug.Log($"player{player.playerIndex +1} is out");
                        player.KnockOut();
                        _miniGame.PlayerKnockedOut();
                        if(!_miniGame.gameRunning)
                            break;
                    }
                }
            }

            _gameTimer -= Time.deltaTime;

            _switchTimer -= Time.deltaTime;

            if (_gameTimer <= 0)
            {
                _miniGame.EndGame();
            }
            if (_switchTimer <= 0)
                SwitchColor();
        }        
    }


    //returns a list of the player indexs in order from closest to furthest
    public List<int> CreatePodium(List<S_RedLightPlayer> players, bool allOut)
    {
        List<int> results = new List<int>();
        players = players.OrderBy( x => Vector2.Distance(this.transform.position, x.transform.position)).ToList();

        if(!allOut)
        {
            foreach (S_RedLightPlayer player in players.FindAll(x => x.isOut)) //moves players that are out to the end of the list
            {
                players.Remove(player);
                players.Add(player);
            }
        }
        
        Debug.Log("game results:");

        foreach(S_RedLightPlayer player in players) //prints players in order of placement
        {
            int position = 1;
            Debug.Log($"{position}: player{player.playerIndex}");
            results.Add(player.playerIndex);
            position++;
        }
        return results;
    }

    public void SwitchColor()
    {
        if (_state >= 3)
            _state = 1;
        else
            _state++;

        switch(_state)
        {
            case 1:
                _renderer.material = _greenMaterial;
                _switchTimer = Random.Range(1, 5);
                break;
            case 2:
                _renderer.material = _yellowMaterial;
                _switchTimer = Random.Range(.5f, 3);
                break;
            case 3:
                _renderer.material = _redMaterial;
                _switchTimer = Random.Range(1, 5);
                break;
        }
        Debug.Log(_state);
    }
}
