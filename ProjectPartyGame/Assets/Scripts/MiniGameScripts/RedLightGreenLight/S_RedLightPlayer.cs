using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_RedLightPlayer : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    private Rigidbody _rb;
    private S_InputController _input;

    [HideInInspector] public bool isOut { get; private set; }
    private void Awake()
    {
        S_GameManager.Instance.currentMiniGame.GetComponent<S_RedLightGame>().players.Add(this);
        _input = GetComponent<S_InputController>();
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!isOut)
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
