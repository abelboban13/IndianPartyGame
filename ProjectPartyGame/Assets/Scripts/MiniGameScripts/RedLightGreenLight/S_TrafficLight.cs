using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_TrafficLight : MonoBehaviour
{

    [SerializeField] private Material _redMaterial;
    [SerializeField] private Material _yelllowMaterial;
    [SerializeField] private Material _greenMaterial;
    [SerializeField] private  S_RedLightGame _miniGame;
    [SerializeField] private float _maxTime = 20;

    private bool _isActive = false;
    private MeshRenderer _renderer;
    private float _gameTimer = 0;
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
        _switchTimer = Random.Range(3, 5);

    }

    // Update is called once per frame
    void Update()
    {
        if(_isActive == false)
        {
            _renderer.material = _redMaterial;
            foreach(var player in _miniGame.players)
            {
                if(player.IsMoving())
                {
                    Debug.Log(player.name + " is out");
                    player.KnockOut();
                    _miniGame.PlayerKnockedOut();
                }
            }
        }
        else
            _renderer.material = _greenMaterial;

        if(_miniGame.gameRunning)
        {
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

    public void SwitchColor()
    {
        _switchTimer = Random.Range(1,5);
        _isActive = !_isActive;
    }
}
