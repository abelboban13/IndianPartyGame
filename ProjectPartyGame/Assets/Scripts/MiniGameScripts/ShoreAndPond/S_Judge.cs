using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    private MeshRenderer _material;
    [SerializeField] private Material shoreColor;
    [SerializeField] private Material pondColor;
    [SerializeField] private TextMeshProUGUI _text;
    // Start is called before the first frame update
    void Start()
    {
        _switchTimer = startingSpeed;
        _detectionTimer = detectionTime;
        _material = GetComponent<MeshRenderer>();
        _material.material = shoreColor;
    }

    // Update is called once per frame
    void Update()
    {
        if(_miniGame.gameRunning && !_miniGame.startPhase)
        {
            _switchTimer -= Time.deltaTime;

            if(_switchTimer <= 0)
            {
                Switch();
                _isDetecting = true;
                startingSpeed = startingSpeed -= _timerDecrease;
                if (startingSpeed < detectionTime)
                    detectionTime = startingSpeed;
                _switchTimer = startingSpeed;
            }

            if(_isDetecting)
            {
                _detectionTimer -= Time.deltaTime;
                if(_detectionTimer <= 0)
                {
                    _detectionTimer = detectionTime;
                    Debug.Log("playerKnockerout");
                    foreach (S_ShoreAndPondPlayer player in _miniGame.players)
                    {
                        if (player.isOut)
                            break;
                        if(player.pond != pond)
                        {
                            player.KnockOut();
                            _miniGame.PlayerKnockedOut();
                        }
                    }
                    _isDetecting = false;
                }
            }
        }
    }

    private void Switch()
    {
        int num = Random.Range(1, 3);
        if (num == 1)
        {
            pond = true;
            _material.material = pondColor;
            _text.text = "Pond";
        }

        else
        {
            pond = false;
            _material.material = shoreColor;
            _text.text = "Shore";
        }
            
    }
}
