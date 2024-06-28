using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Space : MonoBehaviour
{
    [SerializeField] protected SpaceType _spaceType;
    public SpaceType spaceType { get { return _spaceType; } }

    public bool isReward { get; private set; }

    [HideInInspector] public GameObject mango;

    [SerializeField] protected int value = 3;

    [Tooltip("place the object you want to path to. a left input first")]
    [SerializeField] private S_Space[] _nextSpace = new S_Space[2];

    private MeshRenderer _renderer;

    public List<S_BoardPlayer> _playersOnSpace = new List<S_BoardPlayer>();

    public Material[] matList = new Material[6];

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
        _renderer = GetComponent<MeshRenderer>();
        matList = Resources.LoadAll<Material>("SpaceMaterials");
        SetColorBasedOnSpaceType();
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
        {
            foreach(MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
            {
                renderer.enabled = false;
            }
        }
            
        else
        {
            foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
            {
                _renderer.enabled = true;
            }
        }
    }

    public void Load()
    {
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
        {
            _renderer.enabled = true;
        }
    }

    public void UnLoad()
    {
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
        {
            renderer.enabled = false;
        }
    }
    /// <summary>
    /// returns the next connected space
    /// </summary>
    public virtual S_Space GiveNextSpace(S_BoardPlayer player, int direction = 0)
    {
        return _nextSpace[direction];    
    }

    public void SpacePassed(S_BoardPlayer player)
    {
        if(_spaceType == SpaceType.Reward && isReward)
        {
            if (player.coins >= 30)
            {
                player.ChangeCoins(-30);
                player.ChangeMangos(1);
                Debug.Log($"player{player.index} gets a mango!");
                Destroy(mango);
                isReward = false;
                S_SpaceManager.Instance.ManageMangoes();
            }
            else
            {
                player.ChangeCoins(30);
                Debug.Log($"player{player.index} gains {30} coins");
            }     
        }
    }


    // triggered when player lands on the space
    public virtual void SpaceLandedOn(S_BoardPlayer player)
    {
        _playersOnSpace.Add(player);

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
            case SpaceType.Item:
                player.numberOfTraps++;
                Debug.Log($"player{player.index} found a trap!");
                break;
            case SpaceType.Skip:
                player.turnSkipped = true;
                break;
        }
        if(_playersOnSpace.Count > 1)
        {
            // offset players so they dont overlap
            foreach(S_BoardPlayer playerOnSpace in _playersOnSpace)
            {
                //move player random vector
                playerOnSpace.transform.position = playerOnSpace.transform.position + new Vector3(Random.Range(.5f, 1), 0, Random.Range(.5f, 1));
            }
        }



        player.EndTurn();
    }

    public void GenerateReward(GameObject keyObject)
    {
        isReward = true;
        mango = Instantiate(keyObject);
        mango.transform.parent = transform;
        mango.transform.position = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z);
    }

    // Sets the color of the space based on the space type.
    private void SetColorBasedOnSpaceType()
    {
        if (_renderer != null)
        {
            switch (_spaceType)
            {
                case SpaceType.Start:
                    _renderer.material = GetMaterial("M_Default");
                    break;
                case SpaceType.Positive:
                    _renderer.material = GetMaterial("M_CoinsAdded");
                    break;
                case SpaceType.Negative:
                    _renderer.material = GetMaterial("M_CoinsMinus");
                    break;
                case SpaceType.Reward:
                    _renderer.material = GetMaterial("Mangoes");
                    break;
                case SpaceType.Skip:
                    _renderer.material = GetMaterial("M_Default");
                    break;
                case SpaceType.Item:
                    _renderer.material = GetMaterial("Trap");
                    break;
                case SpaceType.Default:
                    _renderer.material = GetMaterial("M_Default");
                    break;
                default:
                    _renderer.material = GetMaterial("M_Default");
                    break;
            }
        }
    }

    // This method is called when the script is loaded or a value changes in the inspector (editor only)
    
    private void OnValidate()
    {
        _renderer = GetComponent<MeshRenderer>();
        matList = Resources.LoadAll<Material>("SpaceMaterials");
        SetColorBasedOnSpaceType();
    }

    private Material GetMaterial(string name)
    {
        foreach (Material mat in matList)
        {
            if (mat.name == name)
                return mat;
        }
        return null;
    }
 
}


    


public enum SpaceType
{
    Positive,
    Negative,
    Reward,
    Start,
    Item,
    Skip,
    Default
}