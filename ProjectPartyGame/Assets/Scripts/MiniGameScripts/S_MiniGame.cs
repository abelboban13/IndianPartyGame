using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_MiniGame : MonoBehaviour
{
    [SerializeField] protected int _reward;

    protected virtual void Start()
    {
        
    }
    public virtual void StartGame()
    {

    }
    public virtual void EndGame()
    {
        S_GameManager.Instance.LoadBoard();
    }
}
