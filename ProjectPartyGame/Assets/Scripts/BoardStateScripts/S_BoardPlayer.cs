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

    public List<S_Item> inventory = new List<S_Item>();

    public Transform cameraSocket;

    public S_Space currentSpace { get; private set; }

    public int coins { get; private set; }

    public int mangos { get; private set; }

    public int numberOfTraps;

    public bool turnSkipped = false;

    public int index;

    public GameObject model;

    private bool _isTurn = false;

    private bool _isMove = false;

    private bool _isUsing = false;

    private S_Space targetSpace;

    private S_InputController _inputController;

    private MeshRenderer _meshRenderer;

    private bool _paused;
    private bool _isUsingCamera;
    [HideInInspector]public bool _itemUsed;

    private Animator _animation;

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
        _animation = GetComponent<Animator>();
        
        //GetComponent<Animation>().clip = S_BoardManager.Instance.playerAnims[index];

    }
    // Start is called before the first frame update
    void Start()
    {
        GameStart();
        AddItem(S_ItemManager.Instance.GetItem(0));
        numberOfTraps += 1;
        coins = 5;
        transform.position = new Vector3(transform.position.x - (index+.5f), transform.position.y, transform.position.z);
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
        if (_isTurn == true &&  !S_BoardUIManager.Instance.paused)
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
            int dieRoll = Random.Range(1, 7);
            StartCoroutine(MoveToNextSpace(dieRoll));
            Debug.Log(dieRoll);
        }
    }

    public void UseItem(int itemIndex)
    {
        _itemUsed = true;
        _inputController.IsConfirm = false;
        S_Item item = inventory[itemIndex];
        S_Projectile proj = Instantiate(inventory[itemIndex].itemPrefab).GetComponent<S_Projectile>();
        proj.player = this;
        proj.transform.position = transform.position;

        item.amount--;
        if(item.amount <= 0)
        {
            inventory.Remove(item);
        }
        _isUsing = false;
    }
    public void UseTrap()
    {
        _itemUsed = false;
        Debug.Log("trap placed");
        numberOfTraps--;
        currentSpace.ActivateTrap();
        _isUsing = false ;
        
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
        if (!_isMove && !_isUsingCamera && !_isUsing)
        {
            if (_inputController.MoveInput != Vector2.zero)
            {
                S_BoardManager.Instance.boardCamera.Pan();
                _isUsingCamera = true;
            }
        }
        else if (_isUsingCamera)
        {
            if (_inputController.IsConfirm || _inputController.IsBack)
            {
                S_BoardManager.Instance.boardCamera.StopPan();
                _isUsingCamera = false;
            }
        }
        if (_itemUsed)
        {
            _inputController.IsConfirm = false;
            return;
        }
        //all player turn options go here
        else if (_inputController.IsBack)
        {
            if(!_isMove && !_isUsing && !_isUsingCamera)
            {
                _isUsing = true;
                S_BoardUIManager.Instance.OpenInventory(this);
            }
            else if(_isUsing)
            {
                S_BoardUIManager.Instance.OpenInventory(this);
                _isUsing = false;
            }   
        }
        if(!_isMove && !_isUsingCamera && !_isUsing)
        {
            if(_inputController.MoveInput != Vector2.zero)
            {
                S_BoardManager.Instance.boardCamera.Pan();
                _isUsingCamera = true;
            }
        }
        
        if(_isMove)
        {
            _animation.SetBool("walking", true);
        }
        else
            _animation.SetBool("walking", false);
        
        if (_inputController.IsConfirm && !_isUsing && !_isUsingCamera)
        {
            RollDice();
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
            if(item.itemName == itemToAdd.itemName)
            {
                item.amount++;
                return; 
            }
        }
        inventory.Add(itemToAdd);
    }
    //handles the player moving a number of spaces
    IEnumerator MoveToNextSpace(int spaces)
    {
        _isMove = true;
        targetSpace = currentSpace.GiveNextSpace();
        for(int i = 0; i < spaces; i++)
        {
            if(currentSpace.NextSpaceNum >= 2)
            {
                Debug.Log("awaiting player input");
                yield return new WaitUntil(() => _inputController.MoveInput.x != 0); 
                targetSpace = currentSpace.GiveNextSpace(_inputController.MoveInput.x > 0 ? 1 :0);//TODO: allow forward y input to return a left choice
            }
            else
            {
                targetSpace = currentSpace.GiveNextSpace();
            }
            //handles the actual movement of the player. rounded so that its not trying to get to an infinitly prisice position
            while ((Mathf.Round(transform.position.x) != Mathf.Round(targetSpace.transform.position.x)) || (Mathf.Round(transform.position.z) != Mathf.Round(targetSpace.transform.position.z)))
            {
                yield return new WaitForFixedUpdate();
                MovePlayer(targetSpace);
                //yield return new WaitForSeconds(.1f);
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
