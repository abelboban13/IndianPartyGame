using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    private TextMeshProUGUI _text;

   [HideInInspector] public S_BoardPlayer player;
   [HideInInspector] public int itemIndex;
   [HideInInspector] public Sprite itemIcon;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //_text.text = player.inventory[0].itemName;
        _text.text = player.inventory[itemIndex].amount.ToString();
        GetComponent<Button>().image.sprite = itemIcon;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pressed()
    {
        player.UseItem(itemIndex);
        S_BoardUIManager.Instance.OpenInventory(player);
    }

    
}
