using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class S_JumpRopePlayer : MonoBehaviour
{
    private S_JumpRopeGame _miniGame;
    [HideInInspector] public int playerIndex;
    [HideInInspector] public bool isOut;
    [SerializeField] private float jumpHeight;
    private S_InputController _input;
    private Rigidbody _rb;
    [HideInInspector] public GameObject model;
    private bool _grounded;
    // Start is called before the first frame update

    private void Awake()
    {
        _miniGame = S_GameManager.Instance.currentMiniGame.GetComponent<S_JumpRopeGame>();
        _input = GetComponent<S_InputController>();
        _miniGame.players.Add(this);
        _rb = GetComponent<Rigidbody>();
        model = Instantiate(S_BoardManager.Instance.playerModels[playerIndex]);
        model.transform.parent = transform;
        model.transform.position = transform.position;
    }
    void Start()
    {
        _grounded = true;
        transform.LookAt(new Vector3(transform.position.x,transform.position.y,-5));
    }

    // Update is called once per frame
    void Update()
    {
        if(_input.IsConfirm && _grounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        _rb.velocity = new Vector3(0, 5,0);
       _grounded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        _grounded = true;
    }

    public void KnockOut()
    {
        isOut = true;
        gameObject.SetActive(false);
        _miniGame.outPlayers.Add(this);
        GetComponent<PlayerInput>().DeactivateInput();
    }

}
