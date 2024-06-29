using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsMenu : MonoBehaviour
{
    private S_MainMenu _menu;

    private void OnEnable()
    {
        _menu = FindAnyObjectByType<S_MainMenu>();
        Debug.Log(_menu.eventSystem.currentSelectedGameObject.name);
    }
 
}
