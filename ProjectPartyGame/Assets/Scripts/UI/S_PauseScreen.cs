using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_PauseScreen : MonoBehaviour
{
    [SerializeField] private S_InputController controller;
    private GameObject defButton;

    private void Start()
    {
        defButton = GetComponentInChildren<Button>().gameObject;
    }

    private void OnEnable()
    {
        S_BoardUIManager.Instance.InputSetUp(defButton);
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        S_BoardUIManager.Instance.UnPause();
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void Quit()
    {
        Application.Quit();
    }

}
