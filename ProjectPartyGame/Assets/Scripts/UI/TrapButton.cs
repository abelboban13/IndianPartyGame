using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TrapButton : MonoBehaviour
{
    [HideInInspector] public S_BoardPlayer player;
    private TextMeshProUGUI _text;
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void MenuClosed()
    {
        gameObject.SetActive(true);
    }

    public void Pressed()
    {
        player.UseTrap();
        S_BoardUIManager.Instance.OpenInventory(player);
    }
    private void OnEnable()
    {
        _text.text = player.numberOfTraps.ToString();
    }
}
