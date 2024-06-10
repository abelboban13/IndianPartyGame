using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_RedLightPlayer : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    private Rigidbody _rb;
    private S_InputController _input;
    private S_RedLightGame _miniGame;
    private Camera _camera;
    [HideInInspector] public int playerIndex;

    [HideInInspector] public bool isOut { get; private set; }
    private void Awake()
    {
        _miniGame = S_GameManager.Instance.currentMiniGame.GetComponent<S_RedLightGame>();
        _miniGame.players.Add(this);
        _input = GetComponent<S_InputController>();
        _rb = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<Camera>();
    }

    private void FixedUpdate()
    {
        if (!isOut && _miniGame.gameRunning)
            _rb.velocity = new Vector3(_input.MoveInput.x * _speed, 0, _input.MoveInput.y * _speed);

    }

    /// <summary>
    /// checks if the player is making an input that moves this object
    /// </summary>
    /// <returns></returns>
    public bool IsMoving()
    {
        return _input.MoveInput.magnitude != 0;
    }

    public void KnockOut()
    {
        isOut = true;
    }
    
}
