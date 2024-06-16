using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Space : MonoBehaviour
{
    [SerializeField] protected SpaceType _spaceType;

    [SerializeField] protected int value = 3;

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
                player.ChangeCoins(value);
                Debug.Log($"player{player.index} gains {value} coins");
                break;
            case SpaceType.Negative:
                player.ChangeCoins(-value);
                Debug.Log($"player{player.index} loses {value} coins");
                break;
            case SpaceType.Reward:
                if(player.coins >= 30)
                {
                    player.ChangeCoins(-30);
                    player.ChangeMangos(1);
                    Debug.Log($"player{player.index} gets a mango!");
                }
                else
                {
                    player.ChangeCoins(30);
                    Debug.Log($"player{player.index} gains {30} coins");
                }

                break;
            case SpaceType.Skip:
                player.turnSkipped = true;
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
    Minigame,
    Skip
}