using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapButton : MonoBehaviour
{
    [HideInInspector] public S_BoardPlayer player;

    public void MenuClosed()
    {
        gameObject.SetActive(true);
    }

    public void Pressed()
    {
        player.UseTrap();
        S_BoardUIManager.Instance.OpenInventory(player);
    }
}
