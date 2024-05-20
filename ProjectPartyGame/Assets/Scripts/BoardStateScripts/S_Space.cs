using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Space : MonoBehaviour
{
    [SerializeField] protected SpaceType _spaceType;

    [SerializeField] protected int value;

    [SerializeField] private S_Space nextSpace;

    // Start is called before the first frame update
    void Start()
    {
        if(_spaceType == SpaceType.Start)
        {
            S_BoardManager.Instance.startingSpace = this;
        }

        switch (_spaceType)
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
                //set color to green
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// returns the next connected space
    /// </summary>
    public virtual S_Space GiveNextSpace()
    {
        return nextSpace;
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
        }

        player.EndTurn();
    }
}

public enum SpaceType
{
    Positive,
    Negative,
    Reward,
    Start
}