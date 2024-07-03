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

    [SerializeField] private int _trapValue = 3;

    [Tooltip("Place the object you want to path to. A left input first.")]
    [SerializeField] private S_Space[] _nextSpace = new S_Space[2];

    private S_ItemManager _itemManager;

    private MeshRenderer _renderer;

    public List<S_BoardPlayer> _playersOnSpace = new List<S_BoardPlayer>();

    public Material[] matList = new Material[6];

    public bool hasTrap;

    [SerializeField] private GameObject trapPrefab; // Reference to the trap prefab
    private GameObject activeTrap; // Instance of the active trap
    private Animator trapAnimator; // Animator component of the trap prefab

    /// <summary>
    /// Returns the number of spaces connected to this one.
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
        _itemManager = S_ItemManager.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_spaceType == SpaceType.Start)
        {
            S_BoardManager.Instance.startingSpace = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (S_GameManager.Instance.GameType != S_GameManager.GameMode.Board)
        {
            foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
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
    /// Returns the next connected space.
    /// </summary>
    public virtual S_Space GiveNextSpace(int direction = 0)
    {
        return _nextSpace[direction];
    }

    public void SpacePassed(S_BoardPlayer player)
    {
        if (_spaceType == SpaceType.Reward && isReward)
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

    // Triggered when player lands on the space
    public virtual void SpaceLandedOn(S_BoardPlayer player)
    {
        _playersOnSpace.Add(player);

        switch (_spaceType)
        {
            case SpaceType.Start:
                break;
            case SpaceType.Positive:
                player.ChangeCoins(value);
                Debug.Log($"player{player.index} gains {value} coins");
                break;
            case SpaceType.Negative:
                player.ChangeCoins(-value);
                Instantiate(S_SpaceManager.Instance.loseParticle);
                Debug.Log($"player{player.index} loses {value} coins");
                break;
            case SpaceType.Item:
                S_Item item = _itemManager.GetRandomItem();
                if (item == null)
                {
                    player.numberOfTraps++;
                    Debug.Log($"player{player.index} found a trap!");
                }
                else
                {
                    player.AddItem(item);
                }
                break;
            case SpaceType.Skip:
                player.turnSkipped = true;
                break;
        }

        if (hasTrap)
        {
            player.ChangeCoins(_trapValue);
            hasTrap = false;
            Debug.Log($"trap triggered! Player{player.index} loses {_trapValue} coins!");
            if (activeTrap != null)
            {
                Destroy(activeTrap); // Destroy the trap prefab instance
            }
        }

        if (_playersOnSpace.Count > 1)
        {
            // Offset players so they don't overlap
            foreach (S_BoardPlayer playerOnSpace in _playersOnSpace)
            {
                // Move player random vector
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

    // Method to activate the trap
    public void ActivateTrap()
    {
        hasTrap = true;
        if (trapPrefab != null)
        {
            activeTrap = Instantiate(trapPrefab, transform.position, Quaternion.identity, transform);
            trapAnimator = activeTrap.GetComponent<Animator>();
            if (trapAnimator != null)
            {
                trapAnimator.enabled = true; // Enable the animator to play the animation
                trapAnimator.Play(0, -1, 0); // Restart the animation to ensure it starts from the beginning
            }
        }
    }

    // Method to deactivate the trap
    public void DeactivateTrap()
    {
        hasTrap = false;
        if (activeTrap != null)
        {
            Destroy(activeTrap);
        }
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
