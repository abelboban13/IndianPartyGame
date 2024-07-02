using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Rope : MonoBehaviour
{
    [SerializeField] private S_JumpRopeGame _miniGame;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedIncrease;
    [SerializeField] private float _timePerIncrease = 1;
    private Transform _pivot;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        _pivot = transform.parent.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(_miniGame.gameRunning & !_miniGame.startPhase)
        {
            timer++;
            if(timer >= _timePerIncrease)
            {
                timer = 0;
                _speed += _speedIncrease;
            }
        }
            
    }

    private void FixedUpdate()
    {
        if (_miniGame.gameRunning & !_miniGame.startPhase)
        {
            _pivot.Rotate(new Vector3(_speed * Time.deltaTime, 0, 0));
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<S_JumpRopePlayer>() != null)
        {
            collision.gameObject.GetComponent<S_JumpRopePlayer>().KnockOut();
            _miniGame.PlayerKnockedOut();
        }
    }
}
