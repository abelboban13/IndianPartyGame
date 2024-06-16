using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_BoardPlayer : MonoBehaviour
{
    [SerializeField] private float _travelDelay = .5f;

    [SerializeField] private float _speed = 5f;

    public Transform cameraSocket;

    public S_Space currentSpace { get; private set; }

    public int coins { get; private set; }

    public int mangos { get; private set; }

    public bool turnSkipped = false;

    public int index;

    private PlayerInput _playerInput;

    private bool _isTurn = false;

    private bool _isMove = false;

    private S_Space targetSpace;

    private S_InputController _inputController;


    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        S_BoardManager.Instance._players.Add(this);
        index = S_BoardManager.Instance._players.Count -1;
        DontDestroyOnLoad(this);
        _inputController = GetComponent<S_InputController>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    { 
        if(_isTurn == true && S_GameManager.Instance.GameType == S_GameManager.GameMode.Board)
        {
            IsTurn();
        }
        if(S_BoardManager.Instance._joining == true)
        {
            if(_inputController.IsConfirm)
            {
                S_BoardManager.Instance.EndJoin();
            }
        }


    }

    /// <summary>
    /// rolls a "1d6" and moves the player that many spaces
    /// </summary>
    private void RollDice()
    {
        if (_isTurn && !_isMove)
        {
            _isTurn = false;
            int dieRoll = Random.Range(1, 6);
            StartCoroutine(MoveToNextSpace(dieRoll));
            Debug.Log(dieRoll);
        }
    }

    public void StartTurn()
    {
        if(turnSkipped)
        {
            Debug.Log($"player{index + 1}'s turn is skipped");
            EndTurn();
        }
        else
        {
            _isTurn = true;
            Debug.Log($"player{index + 1}'s turn");
        }
    }
    
    public void EndTurn()
    {
        _isTurn = false;
        S_BoardManager.Instance.TurnEnd();
    }


    /// <summary>
    /// moves the player towards a space based on their speed
    /// </summary>
    /// <param name="targetSpace"></param>
    public void MovePlayer(S_Space targetSpace)
    {
        var step = _speed * Time.deltaTime; // calculate distance to move
        
        transform.position = Vector3.MoveTowards(transform.position,new Vector3(targetSpace.transform.position.x , transform.position.y, targetSpace.transform.position.z), step);
        transform.LookAt(new Vector3(targetSpace.transform.position.x, transform.position.y, targetSpace.transform.position.z));
    }

    /// <summary>
    /// sets the players space and transform to the starting space
    /// </summary>
    public void GameStart()
    {
        transform.position = new Vector3(S_BoardManager.Instance.startingSpace.transform.position.x, transform.position.y, S_BoardManager.Instance.startingSpace.transform.position.z);
        currentSpace = S_BoardManager.Instance.startingSpace;
    }

    //handles the players turn
    private void IsTurn()
    {
        //all player turn options go here
        if (_inputController.IsConfirm)
        {
            RollDice();
        }
    }

    public void OnReloadBoard()
    {
        _meshRenderer.enabled = true;
    }

    public void OnUnloadBoard()
    {
        if (cameraSocket.GetComponentInChildren<S_BoardCameraController>() != null)
            cameraSocket.GetComponentInChildren<S_BoardCameraController>().DisconnectFromPlayer();

        _meshRenderer.enabled = false;
    }

    public void ChangeCoins(int num)
    {
        coins += num;
    }

    public void ChangeMangos(int num)
    {
        mangos += num;
    }

    //handles the player moving a number of spaces
    IEnumerator MoveToNextSpace(int spaces)
    {
        _isMove = true;
        //Debug.Log(_currentSpace.GiveNextSpace());
        targetSpace = currentSpace.GiveNextSpace(this);
        for(int i = 0; i < spaces; i++)
        {
            if(currentSpace.NextSpaceNum >= 2)
            {
                Debug.Log("awaiting player input");
                yield return new WaitUntil(() => _inputController.MoveInput.x != 0);
                targetSpace = currentSpace.GiveNextSpace(this, _inputController.MoveInput.x > 0 ? 1 :0);
            }
            else
            {
                targetSpace = currentSpace.GiveNextSpace(this);
            }
            //handles the actual movement of the player. rounded so that its not trying to get to an infinitly pricice position
            while ((Mathf.Round(transform.position.x) != Mathf.Round(targetSpace.transform.position.x)) || (Mathf.Round(transform.position.z) != Mathf.Round(targetSpace.transform.position.z)))
            {
                yield return new WaitForFixedUpdate();
                MovePlayer(targetSpace);
                yield return new WaitForSeconds(.1f);
                //Debug.Log(targetSpace);
            }
            currentSpace = targetSpace;
            yield return new WaitForSeconds(_travelDelay);
        }
        currentSpace = targetSpace;
        currentSpace.SpaceLandedOn(this);
        _isMove = false;
        yield return null;
    }

    


}
