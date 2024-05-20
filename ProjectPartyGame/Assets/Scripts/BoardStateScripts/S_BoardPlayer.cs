using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_BoardPlayer : MonoBehaviour
{
    [SerializeField] private float _travelDelay = .5f; 

    private S_Space _currentSpace;

    private bool _isTurn = false;

    private bool _isMove = false;
    private void Awake()
    {
        S_BoardManager.Instance._players.Add(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            RollDice();
        }
    }


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
        _isTurn = true;
    }
    
    public void EndTurn()
    {
        _isTurn = false;
        S_BoardManager.Instance.TurnEnd();
    }

    public void MovePlayer(S_Space targetSpace)
    {
        var step = 500f * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position,new Vector3(targetSpace.transform.position.x , transform.position.y, targetSpace.transform.position.z), step);
       // transform.Translate(new Vector3(targetSpace.transform.position.x - transform.position.x, 0, targetSpace.transform.position.z - transform.position.z) * .5f);
    }

    public void GameStart()
    {
        transform.position = new Vector3(S_BoardManager.Instance.startingSpace.transform.position.x, transform.position.y, S_BoardManager.Instance.startingSpace.transform.position.z);
        _currentSpace = S_BoardManager.Instance.startingSpace;
    }

    IEnumerator MoveToNextSpace(int spaces)
    {
        _isMove = true;
        //Debug.Log(_currentSpace.GiveNextSpace());
        S_Space targetSpace = _currentSpace.GiveNextSpace();
        for(int i = 0; i < spaces; i++)
        {
            targetSpace = _currentSpace.GiveNextSpace();
            while ((Mathf.Round(transform.position.x) != Mathf.Round(targetSpace.transform.position.x)) || (Mathf.Round(transform.position.z) != Mathf.Round(targetSpace.transform.position.z)))
            {
                MovePlayer(targetSpace);
                yield return new WaitForSeconds(.1f);
                //Debug.Log(targetSpace);
            }
            _currentSpace = targetSpace;
            yield return new WaitForSeconds(_travelDelay);
        }
        _currentSpace = targetSpace;
        _currentSpace.SpaceLandedOn(this);
        _isMove = false;
        yield return null;
    }
}
