using Game.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Database", menuName = "Data/Item Database")]
public class ItemsDatabase : ScriptableObject
{
    [SerializeField] private List<ItemData> items;
    public List<ItemData> Items => items;

    public ItemData GetRandomItem()
    {
        int random = Random.Range(0, Items.Count);
        return items[random];
    }
}

[System.Serializable]
public class ItemData
{
    public int item_id;
    public float sprite_size;
    public Sprite item_sprite;
    public BaseItem item_prefab;
}