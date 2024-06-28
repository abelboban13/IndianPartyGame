using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_PauseScreen : MonoBehaviour
{
    [SerializeField] private S_InputController controller;
    [SerializeField] private GameObject _optionsMenu;

    public void Paused(S_InputController input)
    {
        controller = input;
        _optionsMenu.SetActive(false);
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        S_BoardUIManager.Instance.UnPause();
        _optionsMenu.SetActive(false);
    }
    public void Options()
    {
        _optionsMenu.SetActive(true);
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
