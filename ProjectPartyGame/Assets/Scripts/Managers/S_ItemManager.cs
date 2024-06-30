using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ItemManager : S_Singleton<S_ItemManager>
{
    [HideInInspector] public List<S_Item> items = new List<S_Item>();
    // Start is called before the first frame update
    void Start()
    {
       SetItems();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetItems()
    {
        foreach(S_Projectile projectile in Resources.LoadAll<S_Projectile>("Prefabs/Projectiles"))
        {
            items.Add(new S_Item(projectile.name,projectile.gameObject));
        }
    }

    public S_Item GetRandomItem()
    {
        int x = Random.Range(-1, items.Count);
        if(x < 0)
            return null;
        else
            return items[x];
    }
}
