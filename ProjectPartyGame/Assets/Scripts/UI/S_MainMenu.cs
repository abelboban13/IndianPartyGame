using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class S_MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private GameObject _menu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("BoardSetup");
    }

    public void OpenOptions()
    {
        _optionsMenu.SetActive(!_optionsMenu.activeSelf);
        _menu.SetActive(!_menu.activeSelf);
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
