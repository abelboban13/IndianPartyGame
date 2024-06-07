using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Space : MonoBehaviour
{
    [SerializeField] protected SpaceType _spaceType;

    [SerializeField] protected int value;

    [Tooltip("place the object you want to path to from a left input first")]
    [SerializeField] private S_Space[] _nextSpace = new S_Space[2];

    private MeshRenderer _renderer;

    /// <summary>
    /// returns the number of spaces connected to this one
    /// </summary>
    public int NextSpaceNum 
    {
        get
        {
            return _nextSpace.Length;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        _renderer = GetComponent<MeshRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(_spaceType == SpaceType.Start)
        {
            S_BoardManager.Instance.startingSpace = this;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(S_GameManager.Instance.GameType != S_GameManager.GameMode.Board)
            _renderer.enabled = false;
        else
            _renderer.enabled = true;
    }
    /// <summary>
    /// returns the next connected space
    /// </summary>
    public virtual S_Space GiveNextSpace(S_BoardPlayer player, int direction = 0)
    {
        return _nextSpace[direction];    
    }


    /// <summary>
    /// triggered when player lands on the space
    /// </summary>
    public virtual void SpaceLandedOn(S_BoardPlayer player)
    {
        switch(_spaceType)
        {
            case SpaceType.Start:
                break;
            case SpaceType.Positive:
                Debug.Log($"gain {value}");
                break;
            case SpaceType.Negative:
                Debug.Log($"lose {value}");
                break;
            case SpaceType.Reward:
                Debug.Log("get reward!");
                break;
            case SpaceType.Minigame:
                S_GameManager.Instance.LoadMiniGame("SC_MiniGameGym");
                break;
        }

        player.EndTurn();
    }

    

    
}

public enum SpaceType
{
    Positive,
    Negative,
    Reward,
    Start,
    Minigame
}