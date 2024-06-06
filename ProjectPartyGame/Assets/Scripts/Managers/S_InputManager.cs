using System;

using UnityEngine;
using UnityEngine.InputSystem;

public class S_InputManager : S_Singleton<S_InputManager>
{
    private InputActionSystem _actions;
    private PlayerInputManager playerInputManager;

    public static InputAction Move => Instance._actions.Player.Movement;
    public static InputAction Confirm => Instance._actions.Player.Confirm;
    public static InputAction Deny => Instance._actions.Player.Back;

    public static InputAction Start => Instance._actions.Player.Start;

    protected override void OnAwake()
    {
       // Initialize();
        _actions = new InputActionSystem();
        playerInputManager = GetComponent<PlayerInputManager>();
    }


    private void OnEnable() => _actions.Enable();
    //private void OnDisable() => _actions.Disable();


    /* not currently nessacary but might be later
    /// <summary>
    /// Special singleton initializer method.
    /// </summary>
    public new static void Initialize()
    {
        var prefab = Resources.Load<GameObject>("Managers/InputManager");
        if (prefab == null) throw new Exception("Missing InputManager prefab!");

        var instance = Instantiate(prefab);
        if (instance == null) throw new Exception("Failed to instantiate InputManager prefab!");

        instance.name = "Managers.InputManager (Singleton)";
    }
    */
}
