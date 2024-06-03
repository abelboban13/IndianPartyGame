using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_RedLightPlayer : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    private Rigidbody _rb;
    private S_InputController _input;
    private void Awake()
    {
       // S_GameManager.Instance.currentMiniGame.GetComponent<S_TrafficLight>()._players.Add(this);
        _input = GetComponent<S_InputController>();
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //player._rb.velocity = new Vector2(input.MoveInput.x * player.settings.movementSpeed, player._rb.velocity.y);
        _rb.velocity = new Vector3(_input.MoveInput.x * _speed, 0, _input.MoveInput.y * _speed);

    }

    public bool IsMoving()
    {
        return _input.MoveInput.magnitude != 0;
    }
    
}
