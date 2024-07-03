using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class S_ShoreAndPondPlayer : MonoBehaviour
{
    private S_ShoreAndPondGame _miniGame;
    [HideInInspector] public int playerIndex;
    [HideInInspector] public bool isOut;
    private S_InputController _input;
    public bool pond = false;
    private Vector3 _shorePos;
    private Vector3 _pondPos;
    [HideInInspector]public GameObject model;
    // Start is called before the first frame update

    private void Awake()
    {
        _miniGame = S_GameManager.Instance.currentMiniGame.GetComponent<S_ShoreAndPondGame>();
        _input = GetComponent<S_InputController>();
        _miniGame.players.Add(this);
        
    }
    void Start()
    {
        _pondPos = _miniGame.pondSpots[playerIndex].transform.position;
        model = Instantiate(S_BoardManager.Instance.playerModels[playerIndex]);
        model.transform.parent = transform;
        model.transform.position = transform.position;
        _shorePos = transform.position;
        transform.LookAt(_pondPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.IsStart)
        {
            if (S_BoardUIManager.Instance.paused)
                S_BoardUIManager.Instance.UnPause();
            else
                S_BoardUIManager.Instance.PauseGame(_input);
        }
        //player inputs
        if (_input.IsConfirm)
        {
           if(!pond)
           {
                SwapPlaces(false);
           }
        }
        if(_input.IsBack)
        {
            if (pond)
            {
                SwapPlaces(true);
            }
        }
    }

    //moves player to pond if they are on the shore and vice versa
    private void SwapPlaces(bool shore)
    {
        if(shore)
        {
            transform.position = _shorePos;
            transform.LookAt(_pondPos);
            pond = false;
        }
        else
        {
            transform.position = _pondPos;
            transform.LookAt(_shorePos);
            pond = true;
        }
    }

    public void KnockOut()
    {
        isOut = true;
        gameObject.SetActive(false);
        _miniGame.outPlayers.Add(this);
    }
}
