using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_MiniGame : MonoBehaviour
{
    [SerializeField] protected int _reward;

    public bool gameRunning { get; private set; }

    private void Awake()
    {
        S_GameManager.Instance.currentMiniGame = this;
        S_GameManager.Instance.SetGameType(S_GameManager.GameMode.Minigame);
    }

    protected virtual void Start()
    {
        
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
