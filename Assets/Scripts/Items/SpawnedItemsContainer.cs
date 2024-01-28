using Game.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedItemsContainer : MonoBehaviour
{
    [SerializeField] private ItemsDatabase itemsDatabase;
    public ItemsDatabase ItemsDatabase => itemsDatabase;

    private List<BaseItem> spawnedItems = new List<BaseItem>();
    public List<BaseItem> SpawnedItems => spawnedItems;

    public void AddItem(BaseItem item)
    {
        if(!spawnedItems.Contains(item))
        {
            spawnedItems.Add(item);
        }
    }

    public void RemoveItem(BaseItem item) 
    { 
        spawnedItems.Remove(item);
        Destroy(item.gameObject);
    }

    public void Clear()
    {
        foreach (var item in spawnedItems)
        {
            Destroy(item.gameObject);
        }
        spawnedItems.Clear();
    }

    public ItemData RequestItemData()
    {
        if (spawnedItems.Count == 0)
            return null;

        int randomItemIndex = Random.Range(0, spawnedItems.Count);

        ItemData itemData = itemsDatabase.Items[spawnedItems[randomItemIndex].Item_id];
        return itemData;
    }
}
