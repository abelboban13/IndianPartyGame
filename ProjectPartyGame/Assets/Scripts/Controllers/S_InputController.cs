using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_InputController : MonoBehaviour
{
    private S_BoardPlayer player;

    private InputActionAsset _inputActions;
    private InputActionMap _inputMap;

    private InputAction _move;

    private InputAction _confirm;

    private InputAction _back;

    private InputAction _start;


    public Vector2 MoveInput { get; private set; }

    public bool IsConfirm { get; private set; } 

    public bool IsBack { get; private set; }

    public bool IsStart { get; private set; }
    void Awake()
    {
        player = GetComponent<S_BoardPlayer>();
        //set which controller will control this player
        _inputActions = this.GetComponent<PlayerInput>().actions;
        _inputMap = _inputActions.FindActionMap("Player");
    }


    // Update is called once per frame
    void Update()
    {
        MoveInput = _inputMap.FindAction("Movement").ReadValue<Vector2>();   

        IsConfirm = _inputMap.FindAction("Confirm").triggered;

        IsBack = _inputMap.FindAction("Back").triggered;

        IsStart = _inputMap.FindAction("Start").triggered;
    }
}
