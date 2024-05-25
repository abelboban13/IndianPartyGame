using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_InputController : MonoBehaviour
{
    private S_BoardPlayer player;

    public Vector2 MoveInput { get; private set; }

    public bool IsConfirm { get; private set; } 

    public bool IsDeny { get; private set; }
    void Awake()
    {
        player = GetComponent<S_BoardPlayer>();
        //set which controller will control this player
    }

    // Update is called once per frame
    void Update()
    {
        MoveInput = S_InputManager.Move.ReadValue<Vector2>();
    }
}
