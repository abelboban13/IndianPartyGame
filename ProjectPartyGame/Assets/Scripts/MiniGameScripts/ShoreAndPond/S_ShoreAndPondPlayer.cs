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
    // Start is called before the first frame update

    private void Awake()
    {
        _shorePos = transform.position;
        _miniGame = S_GameManager.Instance.currentMiniGame.GetComponent<S_ShoreAndPondGame>();
        _input = GetComponent<S_InputController>();
        _pondPos = _miniGame.pondSpots[playerIndex].transform.position;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //player inputs
        if(_input.IsConfirm)
        {
            Debug.Log("press");
           if(!pond)
           {
                SwapPlaces(false);
           }
        }
        if(_input.IsBack)
        {
            Debug.Log("press");
            if (pond)
            {
                SwapPlaces(true);
            }
        }
    }

    //moves player to pond if they are on the shore and vice versa
    private void SwapPlaces(bool shore)
    {
        Debug.Log("pressed");
        if(shore)
        {
            transform.position = _shorePos;
            pond = false;
        }
        else
        {
            transform.position = _pondPos;
            pond = true;
        }
    }
}
