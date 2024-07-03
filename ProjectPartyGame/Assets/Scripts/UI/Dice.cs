using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Dice : MonoBehaviour
{
    [SerializeField] private Sprite[] die;
    [SerializeField] private Image pic;
    private void Awake()
    {
        pic = GetComponentInChildren<Image>();
        pic.gameObject.SetActive(false);
    }
    public void Roll(int num)
    {
        pic.gameObject.SetActive(true);
        pic.sprite = die[num - 1];
    }
    private void OnEnable()
    {
        
    }
}
