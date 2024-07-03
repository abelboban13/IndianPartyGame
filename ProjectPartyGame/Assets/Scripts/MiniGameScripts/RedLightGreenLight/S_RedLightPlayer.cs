using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XInput;

public class S_RedLightPlayer : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    private Rigidbody _rb;
    private S_InputController _input;
    private S_RedLightGame _miniGame;
    public Camera playerCamera { get; private set; }
    [HideInInspector] public int playerIndex;
    [HideInInspector] public GameObject model;

    [HideInInspector] public bool isOut { get; private set; }
    private void Awake()
    {
        _miniGame = S_GameManager.Instance.currentMiniGame.GetComponent<S_RedLightGame>();
        _miniGame.players.Add(this);
        _input = GetComponent<S_InputController>();
        _rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();
        model = Instantiate(S_BoardManager.Instance.playerModels[playerIndex]);
        model.transform.parent = transform;
        model.transform.position = transform.position;
    }

    private void Update()
    {
        if (_input.IsStart)
        {
            if (S_BoardUIManager.Instance.paused)
                S_BoardUIManager.Instance.UnPause();
            else
                S_BoardUIManager.Instance.PauseGame(_input);
        }
    }
    private void FixedUpdate()
    {
        if (!isOut && _miniGame.gameRunning && !_miniGame.startPhase)
        {
            _rb.velocity = new Vector3(_input.MoveInput.x * _speed, 0, _input.MoveInput.y * _speed);
            transform.LookAt(new Vector3((_input.MoveInput.x + transform.position.x), 0, (_input.MoveInput.y + transform.position.z)));
        }
            
        else
            _rb.velocity = Vector3.zero;

    }

    /// <summary>
    /// checks if the player is making an input that moves this object
    /// </summary>
    /// <returns></returns>
    public bool IsMoving()
    {
        if(!isOut)
            return _input.MoveInput.magnitude != 0;
        else
            return false;
    }

    public void KnockOut()
    {
        isOut = true;
    }
    
}
