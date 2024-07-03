using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class EndScreen : MonoBehaviour
{
    public TextMeshProUGUI playerText;

    private void Start()
    {
        SetWinner(S_BoardManager.Instance.GiveHighestPlayer());
    }
    public void SetWinner(int index)
    {
        switch (index)
        {
            case 0: playerText.text = "Elephant";
                    break;
            case 1: playerText.text = "Gigi";
                    break;
            case 2:
                playerText.text = "Peacock";
                break;
            case 3:
                playerText.text = "Tiger";
                break;
        }
    }
}
