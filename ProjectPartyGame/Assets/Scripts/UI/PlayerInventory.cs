using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PlayerInventory : MonoBehaviour
{
    [SerializeField] GameObject button;
    [SerializeField] GameObject trapButton;
    [HideInInspector] public S_BoardPlayer player;
    private S_InputController controller;
    private List<InventoryButton> _buttons = new List<InventoryButton>();

    private void OnEnable()
    {
      //  controller = player.GetComponent<S_InputController>();
        SetUpInventory();
        if (player.numberOfTraps > 0)
            S_BoardUIManager.Instance.InputSetUp(trapButton, player);
        else
            S_BoardUIManager.Instance.InputSetUp(_buttons[0].gameObject, player);
    }
    private void OnDisable()
    {
        foreach(InventoryButton inventoryButton in _buttons.ToArray())
        {
            _buttons.Remove(inventoryButton);
            Destroy(inventoryButton.gameObject);
        }
        trapButton.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetUpInventory()
    {
        if(player.numberOfTraps > 0)
        {
            trapButton.GetComponent<TrapButton>().player = player;
            trapButton.SetActive(true);
        }
        int index = 0;
        if (player.inventory.Count > 0)
        {
            foreach (S_Item item in player.inventory)
            {
                InventoryButton newButton = Instantiate(button,transform).GetComponent<InventoryButton>();
                _buttons.Add(newButton);
                newButton.player = player;
                newButton.itemIndex = index;
                index++;
            }
        }
    }
}
