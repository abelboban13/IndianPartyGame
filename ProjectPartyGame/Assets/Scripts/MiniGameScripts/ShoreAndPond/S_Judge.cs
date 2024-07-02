using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Judge : MonoBehaviour
{
    public bool pond = false;
    [SerializeField] private S_ShoreAndPondGame _miniGame;
    private float _switchTimer;
    private float _detectionTimer;
    public float startingSpeed;
    [SerializeField] private float _timerDecrease;
    public float detectionTime;
    private bool _isDetecting;
    // Start is called before the first frame update
    void Start()
    {
        _switchTimer = startingSpeed;
        _detectionTimer = detectionTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(_miniGame.gameRunning)
        {
            _switchTimer -= Time.deltaTime;

            if(_switchTimer <= 0)
            {
                Switch();
                _isDetecting = true;
                startingSpeed = startingSpeed -= _timerDecrease;
                _switchTimer = startingSpeed;
            }

            if(_isDetecting)
            {
                _detectionTimer -= Time.deltaTime;
                if(_detectionTimer <= 0)
                {
                    _detectionTimer = detectionTime;
                    foreach(S_ShoreAndPondPlayer player in _miniGame.players)
                    {
                        if (player.isOut)
                            break;
                        if(player.pond != pond)
                        {
                            player.isOut = true;
                        }
                    }
                }
            }
        }
    }

    private void Switch()
    {
        int num = Random.Range(1, 2);
        if (num == 1)
            pond = true;
        else
            pond = false;
    }
}
