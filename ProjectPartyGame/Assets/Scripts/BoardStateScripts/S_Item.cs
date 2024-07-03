using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_Item
{
    public int amount;
    [SerializeField] private string _name;
    public string itemName { get { return _name; } }
    
    public GameObject itemPrefab;

    public Sprite image;

    public S_Item(string name, GameObject item, Sprite uiImage)
    {
        _name = name;
        itemPrefab = item;
        image = uiImage;
    }

    public GameObject GetPrefab()
    {
        foreach(S_Projectile projectile in Resources.LoadAll<S_Projectile>("Prefabs/Projectiles"))
        {
            if(projectile.name == _name)
            {
                return projectile.gameObject;
            }
        }
        return null;
    }
}
