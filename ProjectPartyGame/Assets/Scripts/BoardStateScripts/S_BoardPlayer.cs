using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_BoardPlayer : MonoBehaviour
{
    [SerializeField] private float _travelDelay = .5f;

    [SerializeField] private float _speed = 5f;

    [SerializeField] private S_GameEvent _addEvent;

    public List<S_Item> inventory {  get; private set; }

    public Transform cameraSocket;

    public S_Space currentSpace { get; private set; }

    public int coins { get; private set; }

    public int mangos { get; private set; }

    public int numberOfTraps = 0;

    public bool turnSkipped = false;

    public int index;

    public GameObject model;

    private bool _isTurn = false;

    private bool _isMove = false;

    private S_Space targetSpace;

    private S_InputController _inputController;

    private MeshRenderer _meshRenderer;

    private bool _paused;

    private void Awake()
    {
        S_BoardManager.Instance._players.Add(this);
        index = S_BoardManager.Instance._players.Count -1;
        model = Instantiate(S_BoardManager.Instance.playerModels[index]);
        model.transform.parent = transform;
        model.transform.position = transform.position;
        DontDestroyOnLoad(this);
        _inputController = GetComponent<S_InputController>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _addEvent.Raise();
    }
    // Start is called before the first frame update
    void Start()
    {
        GameStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (S_GameManager.Instance.GameType == S_GameManager.GameMode.Board)
        {
            if (_inputController.IsStart)
            {
                if (S_BoardUIManager.Instance.paused)
                    S_BoardUIManager.Instance.UnPause();
                else
                    S_BoardUIManager.Instance.PauseGame(_inputController);
            }
        }
        if (_isTurn == true &&  !_paused)
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
        S_BoardManager.Instance.boardCamera.FollowPLayer(this);
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
        transform.position = S_BoardManager.Instance.startingSpace.transform.position;//new Vector3(S_BoardManager.Instance.startingSpace.transform.position.x, transform.position.y, S_BoardManager.Instance.startingSpace.transform.position.z);
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
        else if(_inputController.IsBack)
        {
            //use item
            if(numberOfTraps > 0)
            {
                Debug.Log("trap placed");
                numberOfTraps -= 1;
                currentSpace.hasTrap = true;
            }
        }
    }

    public void OnReloadBoard()
    {
        foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
        {
            mesh.enabled = true;
        }
    }

    public void OnUnloadBoard()
    {
        foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
        {
            mesh.enabled = false;
        }
    }

    public void ChangeCoins(int num)
    {
        coins += num;
    }

    public void ChangeMangos(int num)
    {
        mangos += num;
    }

    //adds items to players inventory 
    //if player already has that item it adds to the stored items amount instead
    public void AddItem(S_Item itemToAdd)
    {
        foreach(S_Item item in inventory)
        {
            if(item.GetType() == itemToAdd.GetType())
            {
                item.amount++;
                return; ;
            }
        }
        inventory.Add(itemToAdd);
    }
    //handles the player moving a number of spaces
    IEnumerator MoveToNextSpace(int spaces)
    {
        _isMove = true;
        targetSpace = currentSpace.GiveNextSpace(this);
        for(int i = 0; i < spaces; i++)
        {
            if(currentSpace.NextSpaceNum >= 2)
            {
                Debug.Log("awaiting player input");
                yield return new WaitUntil(() => _inputController.MoveInput.x != 0); 
                targetSpace = currentSpace.GiveNextSpace(this, _inputController.MoveInput.x > 0 ? 1 :0);//TODO: allow forward y input to return a left choice
            }
            else
            {
                targetSpace = currentSpace.GiveNextSpace(this);
            }
            //handles the actual movement of the player. rounded so that its not trying to get to an infinitly prisice position
            while ((Mathf.Round(transform.position.x) != Mathf.Round(targetSpace.transform.position.x)) || (Mathf.Round(transform.position.z) != Mathf.Round(targetSpace.transform.position.z)))
            {
                yield return new WaitForFixedUpdate();
                MovePlayer(targetSpace);
                yield return new WaitForSeconds(.1f);
                //Debug.Log(targetSpace);
            }
            currentSpace = targetSpace;
            currentSpace.SpacePassed(this);
            yield return new WaitForSeconds(_travelDelay);
        }
        currentSpace = targetSpace;
        currentSpace.SpaceLandedOn(this);
        _isMove = false;
        yield return null;
    }

}
