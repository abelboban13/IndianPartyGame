using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class S_MiniGame : MonoBehaviour
{
    [SerializeField] protected int _reward;

    [SerializeField] protected int _startTime = 3;

    [SerializeField] private GameObject _startScreen;
    [SerializeField] private TextMeshProUGUI _countDown;

    public bool gameRunning;

    [HideInInspector] public bool startPhase = false;

    private void Awake()
    {
        S_GameManager.Instance.currentMiniGame = this;
        S_GameManager.Instance.SetGameType(S_GameManager.GameMode.Minigame);
        _countDown.text = "Ready";
    }


    public virtual void LoadGame()
    {
        // gameRunning = true;
        StartCoroutine(StartingPhase());
    }


    

    private void Update()
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

    IEnumerator StartingPhase()
    {
        startPhase = true;
        Debug.Log($"Game starts in: {_startTime}");
        _countDown.text = _startTime.ToString();
        for(int i = 1; i < _startTime; i++)
        {
            yield return new WaitForSeconds(1);
            Debug.Log(_startTime -i);
            _countDown.text = (_startTime - i).ToString();
        }
        yield return new WaitForSeconds(1);
        startPhase = false;
        Debug.Log("Go!");
        _countDown.text = "Go!";
        yield return null;
    }
}
