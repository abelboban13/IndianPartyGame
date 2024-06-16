using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_MiniGame : MonoBehaviour
{
    [SerializeField] protected int _reward;

    public bool gameRunning;

    

    private void Awake()
    {
        S_GameManager.Instance.currentMiniGame = this;
        S_GameManager.Instance.SetGameType(S_GameManager.GameMode.Minigame);
    }

    public virtual void LoadGame()
    {
       // gameRunning = true;
    }

    protected virtual void Start()
    {
        LoadGame(); //move to different function later
    }
    public virtual void StartGame()
    {
        gameRunning = true;
    }
    public virtual void EndGame()
    {
        S_GameManager.Instance.LoadBoard();
    }
}
