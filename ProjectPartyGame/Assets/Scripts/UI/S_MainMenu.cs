using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class S_MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private GameObject _menu;
    [SerializeField] public EventSystem eventSystem;
    // Start is called before the first frame update
    void Start()
    {
        eventSystem = FindAnyObjectByType<EventSystem>();
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
        StartCoroutine(WaitAFrame());
    }


    public void QuitGame()
    {
        Application.Quit();
    }
    IEnumerator WaitAFrame()
    {
        eventSystem.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        eventSystem.SetSelectedGameObject(_optionsMenu.activeSelf ? _menu.GetComponentInChildren<Button>().gameObject : _optionsMenu.GetComponentInChildren<Button>().gameObject);
        _optionsMenu.SetActive(!_optionsMenu.activeSelf);
        _menu.SetActive(!_menu.activeSelf);
        yield return null;
    }
}
